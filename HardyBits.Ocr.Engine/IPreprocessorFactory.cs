using System;
using HardyBits.Wrappers.Leptonica;

namespace HardyBits.Ocr.Engine
{
  public interface IPreprocessorFactory
  {
    IPreprocessor Create(IPreprocessorConfiguration config);
  }

  internal class PreprocessorFactory : IPreprocessorFactory
  {
    private readonly IFilters _filters;

    //public PreprocessorFactory(IFilters filters)
    //{
    //  _filters = filters ?? throw new ArgumentNullException(nameof(filters));
    //}

    public IPreprocessor Create(IPreprocessorConfiguration config)
    {
      return new CloningPreprocessor();
    }
  }

  //public abstract class PreprocessorBase : IPreprocessor
  //{
  //  public IPix Run(IPix image)
  //  {
      
  //  }
  //}

  public class CloningPreprocessor : IPreprocessor
  {
    public IPix Run(IPix image)
    {
      if (image == null)
        throw new ArgumentNullException(nameof(image));

      return image.Clone();
    }
  }

  //public class FilterPreprocessor<T> : IPreprocessor
  // where T : IFilters
  //{
  //  public IPix Run(IPix image)
  //  {
      
  //  }
  //}
}