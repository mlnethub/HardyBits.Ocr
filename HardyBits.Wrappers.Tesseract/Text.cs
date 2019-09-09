using System;
using System.Runtime.InteropServices;
using System.Text;
using HardyBits.Wrappers.Tesseract.Helpers;
using HardyBits.Wrappers.Tesseract.Imports;

namespace HardyBits.Wrappers.Tesseract
{
  public class Text : IDisposable
  {
    private HandleRef _handle;

    private Text(IntPtr textPointer)
    {
      if (textPointer == IntPtr.Zero) 
        throw new ArgumentException("Null text pointer.", nameof(textPointer));

      _handle = new HandleRef(this, textPointer);
    }

    public static Text Create(Func<IntPtr> action)
    {
      var pointer = action();
      return new Text(pointer);
    }

    public override string ToString()
    {
      return NativeHelper.ConvertPointerToString(_handle.Handle, Encoding.UTF8);
    }

    private void ReleaseUnmanagedResources()
    {
      if (_handle.Handle == IntPtr.Zero)
        return;
      
      Tesseract4.TessDeleteText(_handle.Handle);
      _handle = new HandleRef(this, IntPtr.Zero);
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
    }

    ~Text()
    {
      ReleaseUnmanagedResources();
    }
  }
}