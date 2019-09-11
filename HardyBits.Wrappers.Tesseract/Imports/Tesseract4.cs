using System;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Tesseract.Constants;

namespace HardyBits.Wrappers.Tesseract.Imports
{
  internal static class Tesseract4
  {
    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPICreate))]
    public static extern IntPtr TessBaseAPICreate();

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIInit2))]
    public static extern int TessBaseAPIInit2(HandleRef handle, string datapath, string language, int mode);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIDelete))]
    public static extern void TessBaseAPIDelete(HandleRef handle);
    
    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPISetPageSegMode))]
    public static extern void TessBaseAPISetPageSegMode(HandleRef handle, int mode);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPISetImage2))]
    public static extern void TessBaseAPISetImage2(HandleRef handle, HandleRef pix);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIGetUTF8Text))]
    public static extern IntPtr TessBaseAPIGetUTF8Text(HandleRef handle);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessDeleteText))]
    public static extern void TessDeleteText(IntPtr textPtr);
    
    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIRecognize))]
    public static extern int TessBaseAPIRecognize(HandleRef handle, HandleRef monitor);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIRecognize))]
    public static extern int TessBaseAPIRecognize(HandleRef handle, out ETEXT_DESC monitor);
    
    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIGetHOCRText))]
    public static extern IntPtr TessBaseAPIGetHOCRText(HandleRef handle, int page_number);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIClear))]
    public static extern void TessBaseAPIClear(HandleRef handle);
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct EANYCODE_CHAR
  {
    public ushort char_code;
    public short left;
    public short right;
    public short top;
    public short bottom;
    public short font_index;
    public byte confidence;
    public byte point_size;
    public sbyte blanks;
    public byte formatting;
  }

  [StructLayout(LayoutKind.Sequential)]
  public struct ETEXT_DESC 
  {
    public short count;
    public short progress;
    public sbyte more_to_come;
    public sbyte ocr_alive;
    public sbyte err_code;
    public CANCEL_FUNC cancel;
    public PROGRESS_FUNC progress_callback;
    public PROGRESS_FUNC2 progress_callback2;
    public IntPtr cancel_this;
    public TimeSpan end_time;
    public EANYCODE_CHAR[] text;
  }

  public delegate bool CANCEL_FUNC(IntPtr cancel_this, int words);
  public delegate bool PROGRESS_FUNC(int progress, int left, int right, int top, int bottom);
  public delegate bool PROGRESS_FUNC2(ETEXT_DESC ths, int left, int right, int top, int bottom);
}