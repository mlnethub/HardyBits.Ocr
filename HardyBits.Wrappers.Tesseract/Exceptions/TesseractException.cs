using System;

namespace HardyBits.Wrappers.Tesseract.Exceptions
{
  public class TesseractException : Exception
  {
    public TesseractException(string message) : base(message)
    {}
  }
}