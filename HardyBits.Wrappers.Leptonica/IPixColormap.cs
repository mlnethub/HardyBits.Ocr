using System;

namespace HardyBits.Wrappers.Leptonica
{
  public interface IPixColormap : IDisposable
  {
    bool AddColor(IPixColor color);
  }
}