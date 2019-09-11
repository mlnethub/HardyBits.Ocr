using System.Collections.Generic;
using System.Linq;

namespace HardyBits.Ocr.Engine
{
  public interface IValidationResult : IReadOnlyCollection<ValidationProblem>
  {
    bool IsValid { get; }
  }

  public class ValidationResult : List<ValidationProblem>, IValidationResult
  {
    public ValidationResult(IEnumerable<ValidationProblem> collection) 
      : base(collection)
    {
    }

    public bool IsValid => Count == 0;

    public static ValidationResult True = new ValidationResult(Enumerable.Empty<ValidationProblem>());
  }
}