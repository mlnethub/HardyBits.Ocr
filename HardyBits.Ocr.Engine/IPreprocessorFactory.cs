namespace HardyBits.Ocr.Engine
{
  public interface IPreprocessorFactory
  {
    IPreprocessor Create(IPreprocessorConfiguration config);
  }
}