using System;
using HardyBits.Wrappers.Leptonica;

namespace HardyBits.Wrappers.Tesseract
{
  public interface ITesseractEngine : IDisposable
  {
    RecognitionResult Process(Pix image);
  }
}