using System;
using System.IO;
using System.Reflection;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract.Tesseract;
using HardyBits.Wrappers.Tesseract.Tesseract.Enums;

namespace HardyBits.Ocr.Samples.Console
{
  public static class Program
  {
    public static void Main()
    {
      try
      {
        const string testImagePath = "testfile.jpg";

        if (!Pix.IsFileFormatSupported(testImagePath, out _))
        {
          System.Console.WriteLine("Image file format not supported.");
          return;
        }

        var location = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
        var tessDataPath = $@"{location}\libs\tessdata";
        using var engine = new TesseractEngine(tessDataPath, "eng", EngineMode.Default);
        using var img = new Pix(testImagePath);
        using var page = engine.Process(img);
        var text = page.GetText();

        System.Console.WriteLine(text);
        System.Console.ReadLine();
      }
      catch (Exception ex)
      {
        System.Console.WriteLine(ex);
        System.Console.ReadLine();
      }
    }
  }
}
