using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract;
using HardyBits.Wrappers.Tesseract.Constants;
using HardyBits.Wrappers.Tesseract.Enums;
using HardyBits.Wrappers.Tesseract.Results;

namespace HardyBits.Ocr.Engine
{
  public class RecognitionEngine : IRecognitionEngine
  {
    private readonly ActionBlock<Action> _queueBlock;
    private readonly CancellationTokenSource _cancellation;
    private readonly IPreprocessorFactory _preprocessorFactory;
    private readonly ITesseractEngineFactory _tesseractFactory;
    private readonly IPixFactory _pixFactory;

    private int _activeProcessesCount;

    public RecognitionEngine() : this(
      new PreprocessorFactory(), 
      new TesseractEngineFactory(),
      new PixFactory())
    { }

    internal RecognitionEngine(IPreprocessorFactory preprocessorFactory, ITesseractEngineFactory tesseractFactory, IPixFactory pixFactory)
    {
      _preprocessorFactory = preprocessorFactory ?? throw new ArgumentNullException(nameof(preprocessorFactory));
      _tesseractFactory = tesseractFactory ?? throw new ArgumentNullException(nameof(tesseractFactory));
      _pixFactory = pixFactory ?? throw new ArgumentNullException(nameof(pixFactory));

      _cancellation = new CancellationTokenSource();
      var execOptions = new ExecutionDataflowBlockOptions
      {
        MaxDegreeOfParallelism = 10,
        CancellationToken = _cancellation.Token
      };

      _queueBlock = new ActionBlock<Action>(action => action(), execOptions);
    }

    public int ActiveProcessesCount => _activeProcessesCount;
    public int WaitingProcessesCount => _queueBlock.InputCount;

    private bool IsEngineConfigurationSupported(IEngineConfiguration config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (config.Type != Tesseract.Type)
        return false;

      if (!config.Parameters.TryGetValue<string>("tessdata", out var tessdata) || !Directory.Exists(tessdata))
        return false;

      if (!config.Parameters.TryGetValue<string>("language", out var language) || File.Exists($"tessdata/{language}.traineddata"))
        return false;

      if (!config.Parameters.TryGetValue<string>("mode", out var mode) || Enum.GetNames(typeof(EngineMode)).All(x => x != mode))
        return false;

      return true;
    }

    public async Task<IRecognitionResults> RecognizeAsync(IRecognitionConfiguration config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (!IsEngineConfigurationSupported(config.Engine))
        throw new InvalidOperationException("Engine configuration not supported.");

      return await RunProcessAsync(config);
    }

    private async Task<IRecognitionResults> RunProcessAsync(IRecognitionConfiguration config)
    {
      var formatRecognitionBlock = new TransformManyBlock<IImageData, Page<IPix>>(data =>
      {
        var pageNumber = 0;
        var pixes = _pixFactory.Create(data.Data).ToArray();
        return pixes.Select(x => new Page<IPix>(pageNumber++, pixes.Length, x));
      }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10 });

      ISourceBlock<Page<IPix>> previousBlock = formatRecognitionBlock;
      foreach (var configuration in config.Preprocessors)
      {
        var preprocessor = _preprocessorFactory.Create(configuration);

        var transformBlock = new TransformBlock<Page<IPix>, Page<IPix>>(
          page => page.ChangePayload(preprocessor.Run(page.Payload), dispose:true));

        previousBlock.LinkTo(transformBlock);
        previousBlock = transformBlock;
      }

      var recognitionBlock = new TransformBlock<Page<IPix>, Page<IRecognitionResult>>(page =>
      {
        using var tesseract = _tesseractFactory.Create(
          config.Engine.Parameters.GetValue<string>("tessdata"),
          config.Engine.Parameters.GetValue<string>("language"),
          config.Engine.Parameters.GetValue<EngineMode>("mode"));

        var result = tesseract.Process(page.Payload);
        var newPage = page.CloneWithNewPayload(result);
        page.Dispose();
        return newPage;
      });

      _cancellation.Token.ThrowIfCancellationRequested();
      var tcs = new TaskCompletionSource<IRecognitionResults>();
      _cancellation.Token.Register(() => tcs.TrySetCanceled());

      previousBlock.LinkTo(recognitionBlock);
      var pages = new ConcurrentDictionary<int, IRecognitionResult>();
      var bufferBlock = new ActionBlock<Page<IRecognitionResult>>(page =>
      {
        pages.AddOrUpdate(page.CurrentPage, page.Payload,
          (key, __) => throw new InvalidOperationException($"Tried to insert duplicated key {key}."));

        if (pages.Count != page.TotalPages)
          return;

        var results = new RecognitionResults(pages.Values);
        tcs.SetResult(results);
      });

      recognitionBlock.LinkTo(bufferBlock);
      _queueBlock.Post(async () =>
      {
        try
        {
          Interlocked.Increment(ref _activeProcessesCount);

          await formatRecognitionBlock.SendAsync(config.Image);
          formatRecognitionBlock.Complete();
        }
        catch (Exception ex)
        {
          tcs.SetException(ex);
        }
        finally
        {
          Interlocked.Decrement(ref _activeProcessesCount);
        }
      });

      return await tcs.Task;
    }

    public void Dispose()
    {
      _queueBlock.Complete();
      _queueBlock.Completion.Wait();
    }
  }
}