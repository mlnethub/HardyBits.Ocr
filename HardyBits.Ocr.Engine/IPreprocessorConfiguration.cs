namespace HardyBits.Ocr.Engine
{
  public interface IPreprocessorConfiguration : IHaveType, IHaveMethod, IHaveParameters
  {
    IValidationResult Validate();
  }
}