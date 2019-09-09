using System;

namespace HardyBits.Wrappers.Tesseract.Tesseract
{
  public class TesseractException : Exception
  {
    public TesseractException(string message) : base(message)
    {}
  }
}