using System;
using HardyBits.Wrappers.Leptonica;
using HardyBits.Wrappers.Tesseract.Results;

namespace HardyBits.Wrappers.Tesseract
{
  public interface ITesseractEngine : IDisposable
  {
    IRecognitionResult Process(IPix image);
  }
}