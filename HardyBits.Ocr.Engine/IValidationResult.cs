using System.Collections.Generic;

namespace HardyBits.Ocr.Engine
{
  public interface IValidationResult : IReadOnlyCollection<ValidationProblem>
  {
    bool IsValid { get; }
  }
}