using System;

namespace HardyBits.Wrappers.Leptonica
{
  public interface IPixData
  {
    IntPtr Data { get; }
    int WordsPerLine { get; }
  }
}