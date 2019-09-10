using System.Collections.Generic;

namespace HardyBits.Ocr.Engine
{
  public interface IParameterCollection : IReadOnlyDictionary<string, IParameterValue>
  {
  }
}