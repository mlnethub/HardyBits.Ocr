using System;
using System.Runtime.InteropServices;
using System.Threading;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract.Constants;
using HardyBits.Wrappers.Tesseract.Enums;
using HardyBits.Wrappers.Tesseract.Imports;

namespace HardyBits.Wrappers.Tesseract
{
  public class TesseractEngine : ITesseractEngine
  {
    private int _processCount;
    private HandleRef _handle;

    public TesseractEngine(string dataPath, string language, EngineMode engineMode)
    {
      if (dataPath == null)
        throw new ArgumentNullException(nameof(dataPath));

      if (dataPath == string.Empty)
        throw new ArgumentException("Data path is empty.", nameof(dataPath));

      if (language == null)
        throw new ArgumentNullException(nameof(language));

      dataPath = dataPath.Trim();
      if (dataPath.EndsWith("\\", StringComparison.Ordinal) || dataPath.EndsWith("/", StringComparison.Ordinal))
        dataPath = dataPath.Substring(0, dataPath.Length - 1);

      _handle = new HandleRef(this, Tesseract4.TessBaseAPICreate());
      if (Tesseract4.TessBaseAPIInit2(_handle, dataPath, language, (int) engineMode) == 0) 
        return;

      _handle = new HandleRef(this, IntPtr.Zero);
      GC.SuppressFinalize(this);

      throw new TesseractException("Failed to initialise tesseract engine.");
    }

    public IRecognitionResult Process(IPix image)
    {
      if (image == null) 
        throw new ArgumentNullException(nameof(image));

      if (Interlocked.CompareExchange(ref _processCount, 1, 0) != 0)
        throw new InvalidOperationException("Only one image can be processed at once. Please make sure you dispose of the page once your finished with it.");

      try
      {
        const PageSegmentMode pageSegMode = PageSegmentMode.Auto;
        Tesseract4.TessBaseAPISetPageSegMode(_handle, (int) pageSegMode);
        Tesseract4.TessBaseAPISetImage2(_handle, image.Handle);

        if (Tesseract4.TessBaseAPIRecognize(_handle, new HandleRef(this, IntPtr.Zero)) != 0)
          throw new TesseractException("Recognition of image failed.");

        using var text = Text.Create(() => Tesseract4.TessBaseAPIGetUTF8Text(_handle));
        using var hocrText = Text.Create(() => Tesseract4.TessBaseAPIGetHOCRText(_handle, 0));

        return new RecognitionResult 
        { 
          Text = text.ToString(),
          HocrText = $"{Tags.XhtmlBeginTag}{hocrText}{Tags.XhtmlEndTag}"
        };
      }
      finally
      {
        Tesseract4.TessBaseAPIClear(_handle);
        Interlocked.Decrement(ref _processCount);
      }
    }

    private void ReleaseUnmanagedResources()
    {
      if (_handle.Handle == IntPtr.Zero)
        return;

      Tesseract4.TessBaseAPIDelete(_handle);
      _handle = new HandleRef(this, IntPtr.Zero);
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~TesseractEngine()
    {
      ReleaseUnmanagedResources();
    }
  }
}
