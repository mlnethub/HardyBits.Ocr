using System;
using System.IO;
using HardyBits.Ocr.Engine;
using HardyBits.Wrappers.Leptonica;

namespace HardyBits.Ocr.Samples.Console
{
  public static class Program
  {
    public static void Main()
    {
      const string testImagePath = "multipage_tif_example.tif";
      var bytes = File.ReadAllBytes(testImagePath);

      var factory = new PixFactory();
      var pix = factory.Create(bytes.AsMemory());

      //var engine = new RecognitionEngine();

      if(!Pix.IsFileFormatSupported(bytes.AsMemory(), out _))
      {
         System.Console.WriteLine("Image file format not supported.");
         return;
      }

      //try
      //{
      //  const string testImagePath = "testfile.jpg";

      //  if (!Pix.IsFileFormatSupported(testImagePath, out _))
      //  {
      //    System.Console.WriteLine("Image file format not supported.");
      //    return;
      //  }

      //  var location = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
      //  var tessDataPath = $@"{location}\libs\tessdata";
      //  using var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
      //  using var img = new Pix(testImagePath);
      //  var result = engine.Process(img);

      //  System.Console.WriteLine(result.Text);
      //  System.Console.WriteLine(result.HocrText);
      //  System.Console.ReadLine();
      //}
      //catch (Exception ex)
      //{
      //  System.Console.WriteLine(ex);
      //  System.Console.ReadLine();
      //}
    }
  }
}
