using System;
using System.Linq;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Leptonica.Enums;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  public class Pix : IPix
  {
    private static readonly int[] AllowedDepths = { 1, 2, 4, 8, 16, 32 };

    internal Pix(int imageWidth, int imageHeight, int imageDepth)
    {
      if (!AllowedDepths.Contains(imageDepth))
        throw new ArgumentException("Depth must be 1, 2, 4, 8, 16, or 32 bits.", nameof(imageDepth));

      if (imageWidth <= 0) 
        throw new ArgumentException("Width must be greater than zero", nameof(imageWidth));

      if (imageHeight <= 0) 
        throw new ArgumentException("Height must be greater than zero", nameof(imageHeight));

      var handle = Leptonica5Pix.pixCreate(imageWidth, imageHeight, imageDepth);
      if (handle == IntPtr.Zero) 
        throw new InvalidOperationException("Failed to create leptonica pix.");

      Handle = new HandleRef(this, handle);

      Width = imageWidth;
      Height = imageHeight;
      Depth = imageDepth;
    }

    internal Pix(string imageFilePath)
    {
      if (imageFilePath == null)
        throw new ArgumentNullException(nameof(imageFilePath));

      if (!IsFileFormatSupported(imageFilePath, out _))
        throw new ArgumentException("File format not supported or not recognized.", nameof(imageFilePath));

      var handle = Leptonica5Pix.pixRead(imageFilePath);
      if (handle == IntPtr.Zero) 
        throw new InvalidOperationException("Failed to read file.");

      Handle = new HandleRef(this, handle);

      Width = Leptonica5Pix.pixGetWidth(Handle);
      Height = Leptonica5Pix.pixGetHeight(Handle);
      Depth = Leptonica5Pix.pixGetDepth(Handle);
    }

    internal Pix(IntPtr handle)
    {
      if (handle == IntPtr.Zero)
        throw new ArgumentNullException(nameof(handle), "Image pointer is null.");

      Handle = new HandleRef(this, handle);
      Width = Leptonica5Pix.pixGetWidth(Handle);
      Height = Leptonica5Pix.pixGetHeight(Handle);
      Depth = Leptonica5Pix.pixGetDepth(Handle);
    }

    public int Width { get; }
    public int Height { get; }
    public int Depth { get; }

    public static bool IsFileFormatSupported(string filePath, out ImageFileFormat format)
    {
      if (filePath == null)
        throw new ArgumentNullException(nameof(filePath));

      return Leptonica5Pix.findFileFormat(filePath, out format) == 0;
    }

    public static unsafe bool IsFileFormatSupported(ReadOnlyMemory<byte> file, out ImageFileFormat format)
    {
      using var handle = file.Pin();
      return Leptonica5Pix.findFileFormatBuffer(handle.Pointer, out format) == 0;
    }

    public int XRes 
    {
      get => Leptonica5Pix.pixGetXRes(Handle);
      private set => Leptonica5Pix.pixSetXRes(Handle, value);
    }

    public int YRes
    {
      get => Leptonica5Pix.pixGetYRes(Handle);
      private set => Leptonica5Pix.pixSetYRes(Handle, value);
    }

    public HandleRef Handle { get; private set; }

    public IPix Clone()
    {
      var clonePointer = Leptonica5Pix.pixClone(Handle);

      if (clonePointer == IntPtr.Zero)
        throw new InvalidOperationException("Failed to clone pix.");

      return new Pix(clonePointer);
    }

    private void ReleaseUnmanagedResources()
    {
      var tmpHandle = Handle.Handle;
      Leptonica5Pix.pixDestroy(ref tmpHandle);
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