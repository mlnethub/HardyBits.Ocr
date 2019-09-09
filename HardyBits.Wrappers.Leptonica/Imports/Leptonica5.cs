using System;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Leptonica.Constants;
using HardyBits.Wrappers.Leptonica.Enums;

namespace HardyBits.Wrappers.Leptonica.Imports
{
  public static class Leptonica5
  {
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixCreate))]
    public static extern IntPtr pixCreate(int width, int height, int depth);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetColormap))]
    public static extern IntPtr pixGetColormap(HandleRef pix);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixSetColormap))]
    public static extern int pixSetColormap(HandleRef pix, HandleRef colormap);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixDestroyColormap))]
    public static extern int pixDestroyColormap(HandleRef pix);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetXRes))]
    public static extern int pixGetXRes(HandleRef pix);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixSetXRes))]
    public static extern int pixSetXRes(HandleRef pix, int res);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetYRes))]
    public static extern int pixGetYRes(HandleRef pix);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixSetYRes))]
    public static extern int pixSetYRes(HandleRef pix, int res);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixDestroy))]
    public static extern void pixDestroy(ref IntPtr pix);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixcmapCreate))]
    public static extern IntPtr pixcmapCreate(int depth);
    
    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixcmapAddColor))]
    public static extern int pixcmapAddColor(HandleRef cmap, int rval, int gval, int bval);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixcmapDestroy))]
    public static extern void pixcmapDestroy(ref IntPtr pcmap);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetData))]
    public static extern IntPtr pixGetData(HandleRef pix);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetWpl))]
    public static extern int pixGetWpl(HandleRef pix);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(findFileFormat))]
    public static extern int findFileFormat(string filename, out ImageFileFormat pformat);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixRead))]
    public static extern IntPtr pixRead(string filename);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetWidth))]
    public static extern int pixGetWidth(HandleRef pix);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetHeight))]
    public static extern int pixGetHeight(HandleRef pix);

    [DllImport(LibraryNames.Leptonica5, CallingConvention = CallingConvention.Cdecl, EntryPoint = nameof(pixGetDepth))]
    public static extern int pixGetDepth(HandleRef pix);
  }
}