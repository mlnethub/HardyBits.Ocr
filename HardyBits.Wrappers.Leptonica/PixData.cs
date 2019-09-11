using System;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  internal class PixData : IPixData
  {
    public PixData(Pix pix)
    {
      if (pix == null)
        throw new ArgumentNullException(nameof(pix));

      Data = Leptonica5Pix.pixGetData(pix.Handle);
      WordsPerLine = Leptonica5Pix.pixGetWpl(pix.Handle);
    }

    public IntPtr Data { get; }
    public int WordsPerLine { get; }
  }
}