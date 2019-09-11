using System;
using HardyBits.Wrappers.Leptonica.Imports;

namespace HardyBits.Wrappers.Leptonica
{
  public interface IFilters
  {
    IFilterResult Deskew(IPix pix, int searchReductionFactor);
  }

  public class Filters : IFilters
  {
    public IFilterResult Deskew(IPix pix, int searchReductionFactor)
    {
      if (pix == null)
        throw new ArgumentNullException(nameof(pix));

      var ptr = Leptonica5Filters.pixFindSkewAndDeskew(pix.Handle, searchReductionFactor, out var angle, out var confidence);
      if (ptr == IntPtr.Zero)
        throw new InvalidOperationException("Null pointer returned. Operation failed.");

      var newPix = new Pix(ptr);
      return new DeskewFilterResult(newPix, angle, confidence);
    }
  }

  public interface IFilterResult
  {
    IPix Pix { get; }
  }

  public class DeskewFilterResult : IFilterResult
  {
    public DeskewFilterResult(IPix pix, float angle, float confidence)
    {
      Pix = pix ?? throw new ArgumentNullException(nameof(pix));
      Angle = angle;
      Confidence = confidence;
    }

    public IPix Pix { get; }
    public float Angle { get; }
    public float Confidence { get; }
  }
}
