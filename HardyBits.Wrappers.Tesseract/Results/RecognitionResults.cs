using System.Collections.Generic;

namespace HardyBits.Wrappers.Tesseract.Results
{
  public class RecognitionResults : List<IRecognitionResult>, IRecognitionResults
  {
    public RecognitionResults(IEnumerable<IRecognitionResult> collection) : base(collection)
    {
    }
  }
}