using System;

namespace HardyBits.Wrappers.Tesseract
{
  public class TesseractException : Exception
  {
    public TesseractException(string message) : base(message)
    {}
  }
}