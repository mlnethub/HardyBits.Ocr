using System;
using System.Threading.Tasks;
using HardyBits.Wrappers.Tesseract;

namespace HardyBits.Ocr.Engine
{
  public interface IRecognitionEngine : IDisposable
  {
    int ActiveProcessesCount { get; }
    int WaitingProcessesCount { get; }
    bool IsImageFormatSupported(ReadOnlyMemory<byte> file);
    bool IsEngineConfigurationSupported(IEngineConfiguration config);
    Task<IRecognitionResults> RecognizeAsync(IRecognitionConfiguration config);
  }
}
