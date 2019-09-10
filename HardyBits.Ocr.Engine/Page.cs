using System;

namespace HardyBits.Ocr.Engine
{
  internal class Page<T> : IDisposable
    where T : class, IDisposable
  {
    public int TotalPages { get; }
    public int CurrentPage { get; }
    public T Payload { get; private set; }

    public Page(int currentPage, int totalPages, T payload)
    {
      if (totalPages <= 0)
        throw new ArgumentException("Total pages must have value higher than 0.");

      if (currentPage >= 0)
        throw new ArgumentException("Current page must have value higher or equal to 0.");

      if (currentPage >= totalPages)
        throw new ArgumentOutOfRangeException(nameof(currentPage), currentPage, "Current page value is higher that total pages value.");

      TotalPages = totalPages;
      CurrentPage = currentPage;
      Payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public Page<T> ChangePayload(T newPayload, bool dispose = false)
    {
      if (newPayload == null)
        throw new ArgumentNullException(nameof(newPayload));

      var previousPayload = Payload;
      Payload = newPayload;

      if (dispose)
        previousPayload.Dispose();

      return this;
    }

    public Page<TNew> CloneWithNewPayload<TNew>(TNew newPayload)
      where TNew : class, IDisposable
    {
      if (newPayload == null)
        throw new ArgumentNullException(nameof(newPayload));

      return new Page<TNew>(CurrentPage, TotalPages, newPayload);
    }

    public void Dispose()
    {
      var payload = Payload;
      Payload = null;
      payload?.Dispose();
    }
  }
}