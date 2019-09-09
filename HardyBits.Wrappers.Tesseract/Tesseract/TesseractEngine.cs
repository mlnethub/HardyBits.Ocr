using System;
using System.Runtime.InteropServices;
using System.Threading;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract.Tesseract.Enums;
using HardyBits.Wrappers.Tesseract.Tesseract.Imports;

namespace HardyBits.Wrappers.Tesseract.Tesseract
{
  public class TesseractEngine : IDisposable
  {
    private int _processCount;

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
        dataPath = dataPath[..^1];

      Handle = new HandleRef(this, Tesseract4.TessBaseAPICreate());
      if (Tesseract4.TessBaseAPIInit2(Handle, dataPath, language, (int) engineMode) == 0) 
        return;

      Handle = new HandleRef(this, IntPtr.Zero);
      GC.SuppressFinalize(this);

      throw new TesseractException("Failed to initialise tesseract engine.");
    }

    internal HandleRef Handle { get; private set; }

    public Page Process(Pix image)
    {
      if (image == null) 
        throw new ArgumentNullException(nameof(image));

      if (Interlocked.CompareExchange(ref _processCount, 1, 0) != 0)
        throw new InvalidOperationException("Only one image can be processed at once. Please make sure you dispose of the page once your finished with it.");

      const PageSegmentMode pageSegMode = PageSegmentMode.Auto;
      Tesseract4.TessBaseAPISetPageSegMode(Handle, (int) pageSegMode);
      Tesseract4.TessBaseAPISetImage2(Handle, image.Handle);

      return new Page(this, pageSegMode, DecrementProcessCount);
    }

    private void DecrementProcessCount()
    {
      Interlocked.Decrement(ref _processCount);
    }

    private void ReleaseUnmanagedResources()
    {
      if (Handle.Handle == IntPtr.Zero)
        return;

      Tesseract4.TessBaseAPIDelete(Handle);
      Handle = new HandleRef(this, IntPtr.Zero);
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
