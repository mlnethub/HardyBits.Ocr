using System;

namespace HardyBits.Wrappers.Tesseract
{
  public class RecognitionResult
  {
    public RecognitionResult(string text, string hocrText)
    {
      Text = text ?? throw new ArgumentNullException(nameof(text));
      HocrText = hocrText ?? throw new ArgumentNullException(nameof(hocrText));
    }

    public string Text { get; }
    public string HocrText { get; }
  }
}