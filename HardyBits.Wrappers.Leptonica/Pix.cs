using System;
using System.Linq;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Leptonica.Enums;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  public class Pix : IDisposable
  {
    private static readonly int[] AllowedDepths = { 1, 2, 4, 8, 16, 32 };
    private PixColormap _colormap;

    public Pix(int imageWidth, int imageHeight, int imageDepth)
    {
      if (!AllowedDepths.Contains(imageDepth))
        throw new ArgumentException("Depth must be 1, 2, 4, 8, 16, or 32 bits.", nameof(imageDepth));

      if (imageWidth <= 0) 
        throw new ArgumentException("Width must be greater than zero", nameof(imageWidth));

      if (imageHeight <= 0) 
        throw new ArgumentException("Height must be greater than zero", nameof(imageHeight));

      var handle = Leptonica5.pixCreate(imageWidth, imageHeight, imageDepth);
      if (handle == IntPtr.Zero) 
        throw new InvalidOperationException("Failed to create leptonica pix.");

      Handle = new HandleRef(this, handle);

      Width = imageWidth;
      Height = imageHeight;
      Depth = imageDepth;

      var colorMapHandle = Leptonica5.pixGetColormap(Handle);
      if (colorMapHandle != IntPtr.Zero)
        _colormap = new PixColormap(colorMapHandle);
    }

    public Pix(string imageFilePath)
    {
      if (imageFilePath == null)
        throw new ArgumentNullException(nameof(imageFilePath));

      if (!IsFileFormatSupported(imageFilePath, out _))
        throw new ArgumentException("File format not supported or not recognized.", nameof(imageFilePath));

      var handle = Leptonica5.pixRead(imageFilePath);
      if (handle == IntPtr.Zero) 
        throw new InvalidOperationException("Failed to read file.");

      Handle = new HandleRef(this, handle);

      Width = Leptonica5.pixGetWidth(Handle);
      Height = Leptonica5.pixGetHeight(Handle);
      Depth = Leptonica5.pixGetDepth(Handle);

      var colorMapHandle = Leptonica5.pixGetColormap(Handle);
      if (colorMapHandle != IntPtr.Zero)
        _colormap = new PixColormap(colorMapHandle);
    }

    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }

    public static bool IsFileFormatSupported(string filePath, out ImageFileFormat format)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof(filePath));

      return Leptonica5.findFileFormat(filePath, out format) == 0;

    }

    public PixColormap Colormap
    {
      get => _colormap;
      set
      {
        if (value == null)
        {
          if (Leptonica5.pixDestroyColormap(Handle) == 0)
            _colormap = null;
        }
        else
        {
          if (Leptonica5.pixSetColormap(Handle, value.Handle) == 0)
            _colormap = value;
        }
      }
    }

    public int XRes 
    {
      get => Leptonica5.pixGetXRes(Handle);
      set => Leptonica5.pixSetXRes(Handle, value);
    }

    public int YRes
    {
      get => Leptonica5.pixGetYRes(Handle);
      set => Leptonica5.pixSetYRes(Handle, value);
    }

    public HandleRef Handle { get; private set; }

    public PixData GetData()
    {
      return new PixData(this);
    }

    private void ReleaseUnmanagedResources()
    {
      _colormap?.Dispose();
      var tmpHandle = Handle.Handle;
      Leptonica5.pixDestroy(ref tmpHandle);
      Handle = new HandleRef(this, IntPtr.Zero);
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~Pix()
    {
      ReleaseUnmanagedResources();
    }
  }
}