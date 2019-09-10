using HardyBits.Wrappers.Tesseract.Enums;

namespace HardyBits.Wrappers.Tesseract
{
  public interface ITesseractEngineFactory
  {
    ITesseractEngine Create(string dataPath, string language, EngineMode engineMode);
  }
}