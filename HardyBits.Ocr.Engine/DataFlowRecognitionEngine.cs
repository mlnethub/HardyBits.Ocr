using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract;

namespace HardyBits.Ocr.Engine
{
  public class DataFlowRecognitionEngine : IRecognitionEngine
  {
    private readonly ActionBlock<Action> _queueBlock;
    private readonly CancellationTokenSource _cancellation;
    private readonly IPreprocessorFactory _preprocessorFactory;

    private int _activeProcessesCount;

    public DataFlowRecognitionEngine(IPreprocessorFactory preprocessorFactory)
    {
      _preprocessorFactory = preprocessorFactory ?? throw new ArgumentNullException(nameof(preprocessorFactory));

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

    public bool IsImageFormatSupported(ReadOnlyMemory<byte> file)
    {
      return Pix.IsFileFormatSupported(file, out _);
    }

    public bool IsEngineConfigurationSupported(IEngineConfiguration config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      // ToDo
      return false;
    }

    public async Task<IRecognitionResults> RecognizeAsync(IRecognitionConfiguration config)
    {
      if (config == null)
        throw new ArgumentNullException(nameof(config));

      if (!IsImageFormatSupported(config.Image.Data))
        throw new InvalidOperationException("Image format not supported.");

      if (!IsEngineConfigurationSupported(config.Engine))
        throw new InvalidOperationException("Engine configuration not supported.");

      return await RunProcessAsync(config);
    }

    private async Task<IRecognitionResults> RunProcessAsync(IRecognitionConfiguration config)
    {
      var formatRecognitionBlock = new TransformManyBlock<IImageData, Page<IPix>>(data =>
      {
        // ToDo
        return new Page<IPix>[0];
      }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 10 });

      ISourceBlock<Page<IPix>> previousBlock = formatRecognitionBlock;
      foreach (var configuration in config.Preprocessors)
      {
        var result = configuration.Validate();

        if (!result.IsValid)
          throw new InvalidOperationException($"Preprocessor ({configuration.Type} {configuration.Method}) configuration is invalid.");

        var preprocessor = _preprocessorFactory.Create(configuration);

        var transformBlock = new TransformBlock<Page<IPix>, Page<IPix>>(
          page => page.ChangePayload(preprocessor.Run(page.Payload), dispose:true));

        previousBlock.LinkTo(transformBlock);
        previousBlock = transformBlock;
      }

      var recognitionBlock = new TransformBlock<Page<IPix>, Page<IRecognitionResult>>(pix =>
      {
        return (Page<IRecognitionResult>) null;
      });

      previousBlock.LinkTo(recognitionBlock);
      var bufferBlock = new BufferBlock<Page<IRecognitionResult>>();

      _cancellation.Token.ThrowIfCancellationRequested();
      var tcs = new TaskCompletionSource<IRecognitionResults>();
      _cancellation.Token.Register(() => tcs.SetCanceled());

      _queueBlock.Post(async () =>
      {
        try
        {
          Interlocked.Increment(ref _activeProcessesCount);

          await formatRecognitionBlock.SendAsync(config.Image);
          formatRecognitionBlock.Complete();
          await bufferBlock.Completion;
          bufferBlock.TryReceiveAll(out var result);
          var results = result.OrderBy(x => x.CurrentPage).Select(x => x.Payload).ToArray();
          // ToDo: sprawdzenie liczby
          tcs.SetResult(new RecognitionResults(results));
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
      _cancellation.Cancel();
      _queueBlock.Completion.Wait();
    }
  }
}