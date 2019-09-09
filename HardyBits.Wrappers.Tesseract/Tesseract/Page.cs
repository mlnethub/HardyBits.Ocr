using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using HardyBits.Wrappers.Tesseract.Constants;
using HardyBits.Wrappers.Tesseract.Helpers;
using HardyBits.Wrappers.Tesseract.Tesseract.Enums;
using HardyBits.Wrappers.Tesseract.Tesseract.Imports;

namespace HardyBits.Wrappers.Tesseract.Tesseract
{
  public class Page : IDisposable
  {
    private TesseractEngine _engine;
    private Action _disposeAction;
    private readonly PageSegmentMode _pageSegmentMode;
    private int _runRecognitionPhase;

    internal Page(TesseractEngine engine, PageSegmentMode pageSegmentMode, Action disposeAction)
    {
      _engine = engine ?? throw new ArgumentNullException(nameof(engine));
      _disposeAction = disposeAction ?? throw new ArgumentNullException(nameof(disposeAction));
      _pageSegmentMode = pageSegmentMode;
    }

    public string GetText()
    {
      Recognize();
      var txtHandle = Tesseract4.TessBaseAPIGetUTF8Text(_engine.Handle);
      return HandleNativeTextResult(txtHandle);
    }

    public string GetHOcr()
    {
      Recognize();
      var txtHandle = Tesseract4.TessBaseAPIGetHOCRText(_engine.Handle, 0);
      return $"{Tags.XhtmlBeginTag}{HandleNativeTextResult(txtHandle)}{Tags.XhtmlEndTag}";
    }

    private static string HandleNativeTextResult(IntPtr txtHandle)
    {
      if (txtHandle == IntPtr.Zero) 
        throw new TesseractException("Null pointer returned.");

      var result = NativeHelper.ConvertPointerToString(txtHandle, Encoding.UTF8);
      Tesseract4.TessDeleteText(txtHandle);
      return result;
    }

    private void Recognize()
    {
      if (_pageSegmentMode == PageSegmentMode.OsdOnly)
        throw new InvalidOperationException("Cannot OCR image when using OSD only page segmentation, please use DetectBestOrientation instead.");

      if (Interlocked.CompareExchange(ref _runRecognitionPhase, 1, 0) == 1)
        return;

      if (Tesseract4.TessBaseAPIRecognize(_engine.Handle, new HandleRef(this, IntPtr.Zero)) != 0)
        throw new TesseractException("Recognition of image failed.");
    }

    private void ReleaseUnmanagedResources()
    {
      Tesseract4.TessBaseAPIClear(_engine.Handle);
      _engine = null;
      _disposeAction();
      _disposeAction = null;
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~Page()
    {
      ReleaseUnmanagedResources();
    }
  }
}