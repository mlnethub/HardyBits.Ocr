using System;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Tesseract.Constants;

namespace HardyBits.Wrappers.Tesseract.Tesseract.Imports
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
    
    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIGetHOCRText))]
    public static extern IntPtr TessBaseAPIGetHOCRText(HandleRef handle, int page_number);

    [DllImport(LibraryNames.Tesseract4, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(TessBaseAPIClear))]
    public static extern void TessBaseAPIClear(HandleRef handle);
  }
}