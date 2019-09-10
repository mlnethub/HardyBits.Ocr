using System;
using System.Threading.Tasks;
using HardyBits.Wrappers.Tesseract;
using HardyBits.Wrappers.Tesseract.Results;

namespace HardyBits.Ocr.Engine
{
  public interface IRecognitionEngine : IDisposable
  {
    int ActiveProcessesCount { get; }
    int WaitingProcessesCount { get; }
    Task<IRecognitionResults> RecognizeAsync(IRecognitionConfiguration config);
  }
}
