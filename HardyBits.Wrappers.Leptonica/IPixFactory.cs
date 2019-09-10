using System;
using System.Collections.Generic;
using System.Linq;
using HardyBits.Wrappers.Leptonica.Enums;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  public interface IPixFactory
  {
    IEnumerable<IPix> Create(ReadOnlyMemory<byte> dataData);
  }

  public class PixFactory : IPixFactory
  {
    public unsafe IEnumerable<IPix> Create(ReadOnlyMemory<byte> data)
    {
      using var pointer = data.Pin();
      if (Leptonica5.findFileFormatBuffer(pointer.Pointer, out var format) != 0)
        throw new InvalidOperationException("File format not supported.");

      var sizeInMemory = sizeof(byte) * data.Length;

      switch (format)
      {
        case ImageFileFormat.Tiff:
        case ImageFileFormat.TiffPackbits:
        case ImageFileFormat.TiffRle:
        case ImageFileFormat.TiffG3:
        case ImageFileFormat.TiffG4:
        case ImageFileFormat.TiffLzw:
        case ImageFileFormat.TiffZip:
          return ReadTiff(pointer.Pointer, sizeInMemory);
        default:
          return Enumerable.Repeat(ReadImage(pointer.Pointer, sizeInMemory), 1);
      }
    }

    private static unsafe IPix ReadImage(void* pointer, int size)
    {
      var pixPointer = Leptonica5.pixReadMem(pointer, size);

      if (pixPointer == IntPtr.Zero)
        throw new InvalidOperationException("Failed to load image.");

      return new Pix(pixPointer);
    }

    private static unsafe IEnumerable<IPix> ReadTiff(void* pointer, int size)
    {
      var pixaPointer = Leptonica5.pixaReadMemMultipageTiff(pointer, size);
      if (pixaPointer == IntPtr.Zero)
        throw new InvalidOperationException("Failed to load image.");

      try
      {
        var pagesCount = Leptonica5.pixaGetCount(pixaPointer);
        if (pagesCount <= 0)
          throw new InvalidOperationException("File has no pages.");

        return Enumerable.Range(0, pagesCount)
          .Select(index => new Pix(Leptonica5.pixaGetPix(pixaPointer, index, 2)))
          .ToArray();

        // ToDo: failure path
      }
      finally
      {
        Leptonica5.pixaDestroy(ref pixaPointer);
      }
    }
  }
}