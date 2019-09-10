using HardyBits.Wrappers.Leptonica;

namespace HardyBits.Ocr.Engine
{
  public interface IPreprocessor
  {
    IPix Run(IPix image);
  }
}