using System;

namespace HardyBits.Ocr.Engine
{
  public interface IImageData
  {
    string Name { get; }
    string Extension { get; }
    string MimeType { get; }
    ReadOnlyMemory<byte> Data { get; }
  }
}