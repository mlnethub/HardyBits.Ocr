using System;

namespace HardyBits.Wrappers.Leptonica
{
  public interface IPixColor : IEquatable<IPixColor>
  {
    byte Red { get; }
    byte Green { get; }
    byte Blue { get; }
    byte Alpha { get; }
  }
}