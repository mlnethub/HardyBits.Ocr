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

  public class ParameterValue<T> : IParameterValue<T>
  {
    public ParameterValue(T value)
    {
      Value = value;
    }

    public T Value { get; }
    public object BoxedValue => Value;

    public override string ToString()
    {
      return Value?.ToString() ?? "null";
    }
  }

  public static class ParameterValue
  {
    public static IParameterValue<T> Create<T>(T value)
    {
      return new ParameterValue<T>(value);
    }
  }
}