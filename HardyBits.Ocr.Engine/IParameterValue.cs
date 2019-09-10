namespace HardyBits.Ocr.Engine
{
  public interface IParameterValue
  {
    object BoxedValue { get; }
  }

  public interface IParameterValue<out T> : IParameterValue
  {
    T Value { get; }
  }
}