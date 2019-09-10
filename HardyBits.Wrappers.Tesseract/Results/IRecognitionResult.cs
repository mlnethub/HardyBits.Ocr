using System;

namespace HardyBits.Wrappers.Tesseract.Results
{
  public interface IRecognitionResult : IDisposable
  {
    string Text { get; }
    string HocrText { get; }
    float? Confidence { get; }
  }
}