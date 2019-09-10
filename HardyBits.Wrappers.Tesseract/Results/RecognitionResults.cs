using System.Collections.Generic;

namespace HardyBits.Wrappers.Tesseract
{
  public class RecognitionResults : List<IRecognitionResult>, IRecognitionResults
  {
    public RecognitionResults(IEnumerable<IRecognitionResult> collection) : base(collection)
    {
    }
  }
}