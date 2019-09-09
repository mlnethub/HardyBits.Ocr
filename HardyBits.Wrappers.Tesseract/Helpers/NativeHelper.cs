using System;
using System.Text;

namespace HardyBits.Wrappers.Tesseract.Helpers
{
  public static unsafe class NativeHelper
  {
    public static string ConvertPointerToString(IntPtr handle, Encoding encoding)
    {
      var length = GetNativeStringLength(handle);
      return new string((sbyte*) handle.ToPointer(), 0, length, encoding);
    }

    public static int GetNativeStringLength(IntPtr handle)
    {
      var ptr = (byte*) handle.ToPointer();
      var length = 0;

      while (*(ptr + length) != 0)
        length++;

      return length;
    }
  }
}
