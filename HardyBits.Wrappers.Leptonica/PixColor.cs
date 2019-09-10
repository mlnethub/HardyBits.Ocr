using System.Drawing;
using System.Runtime.InteropServices;

namespace HardyBits.Wrappers.Leptonica
{
  [StructLayout(LayoutKind.Sequential, Pack=1)]
  internal class PixColor : IPixColor
  {
    private readonly byte red;
    private readonly byte blue;
    private readonly byte green;
    private readonly byte alpha;

    public PixColor(byte r, byte g, byte b, byte a = 255)
    {
      red = r;
      green = g;
      blue = b;
      alpha = a;
    }

    public byte Red => red;
    public byte Green => green;
    public byte Blue => blue;
    public byte Alpha => alpha;

    public static explicit operator PixColor(Color color)
    {
      return new PixColor(color.R, color.G, color.B, color.A);
    }

    public bool Equals(IPixColor other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return red == other.Red && blue == other.Blue && green == other.Green && alpha == other.Alpha;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((IPixColor) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = red.GetHashCode();
        hashCode = (hashCode * 397) ^ blue.GetHashCode();
        hashCode = (hashCode * 397) ^ green.GetHashCode();
        hashCode = (hashCode * 397) ^ alpha.GetHashCode();
        return hashCode;
      }
    }
  }
}