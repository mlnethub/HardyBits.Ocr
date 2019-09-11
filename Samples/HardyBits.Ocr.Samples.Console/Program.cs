using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HardyBits.Ocr.Engine;

namespace HardyBits.Ocr.Samples.Console
{
  public static class Program
  {
    private class RecognitionConfiguration : IRecognitionConfiguration
    {
      private class ImageData : IImageData
      {
        public string Name { get; } = "multipage_tif_example";
        public string Extension { get; } = "tif";
        public string MimeType { get; } = "image/tiff";
        public ReadOnlyMemory<byte> Data { get; } = File.ReadAllBytes("testfile.jpg");
      }

      private class EngineConfiguration : IEngineConfiguration
      {
        public string Type { get; } = "tesseract4";
        public IParameterCollection Parameters { get; } = new ParameterCollection
        {
          {"language", ParameterValue.Create("eng")},
          {"mode", ParameterValue.Create("Default")},
          {"tessdata", ParameterValue.Create($@"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}\libs\tessdata")}
        };
      }

      private class PreprocessorConfiguration : IPreprocessorConfiguration
      {
        public string Type { get; } = "Any";
        public string Method { get; } = "Any";
        public IParameterCollection Parameters { get; } = new ParameterCollection();
      }

      public RecognitionConfiguration()
      {
        Engine = new EngineConfiguration();
        Image = new ImageData();
        Preprocessors = Enumerable.Repeat(new PreprocessorConfiguration(), 1).ToArray();
      }

      public IImageData Image { get; }
      public IEngineConfiguration Engine { get; }
      public IReadOnlyCollection<IPreprocessorConfiguration> Preprocessors { get; }
    }

    public static async Task Main()
    {
      using var engine = new RecognitionEngine();
      var config = new RecognitionConfiguration();
      var result = await engine.RecognizeAsync(config);

      foreach (var page in result)
      {
        System.Console.WriteLine(page.Text);
        System.Console.WriteLine(page.HocrText);
        System.Console.WriteLine(page.Confidence);
      }

      System.Console.ReadLine();
    }
  }
}
