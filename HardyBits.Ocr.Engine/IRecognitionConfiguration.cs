using System.Collections.Generic;

namespace HardyBits.Ocr.Engine
{
  public interface IRecognitionConfiguration
  {
    IImageData Image { get; }
    IEngineConfiguration Engine { get; }
    IReadOnlyCollection<IPreprocessorConfiguration> Preprocessors { get; }
  }
}