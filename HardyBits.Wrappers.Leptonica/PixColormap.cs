using System;
using System.Linq;
using System.Runtime.InteropServices;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  internal class PixColormap : IPixColormap
  {
    private static readonly int[] AcceptedDepths = {1, 2, 4, 8};

    internal PixColormap(IntPtr handle)
    {
      Handle = new HandleRef(this, handle);
    }

    public PixColormap(int depth)
    {
      if (!AcceptedDepths.Contains(depth))
        throw new ArgumentOutOfRangeException(nameof(depth), "Depth must be 1, 2, 4, or 8 bpp.");

      var handle = Leptonica5Pix.pixcmapCreate(depth);
      if (handle == IntPtr.Zero) {
        throw new InvalidOperationException("Failed to create colormap.");
      }
      
      Handle = new HandleRef(this, handle);
    }

    public HandleRef Handle { get; private set; }

    public bool AddColor(IPixColor color)
    {
      return Leptonica5Pix.pixcmapAddColor(Handle, color.Red, color.Green, color.Blue) == 0;
    }

    private void ReleaseUnmanagedResources()
    {
      var tmpHandle = Handle.Handle;
      Leptonica5Pix.pixcmapDestroy(ref tmpHandle);
      Handle = new HandleRef(this, IntPtr.Zero);
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~PixColormap()
    {
      ReleaseUnmanagedResources();
    }
  }
}