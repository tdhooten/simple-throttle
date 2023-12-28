namespace SimpleThrottle;

public class Throttler(int requestLimit, int timespanInMilliseconds) : IDisposable
{
    private DateTime _lastRequestTime = DateTime.MinValue;
    private int _requestCount = 0;
    private readonly int _requestLimit = requestLimit;
    private readonly int _timespanInMilliseconds = timespanInMilliseconds;
    private readonly SemaphoreSlim _semaphore = new(requestLimit, requestLimit);
    private readonly object _lockObject = new();

    public void MakeRequest()
    {
        _semaphore.Wait();

        // Make sure to update the request count and last request time in a thread-safe manner
        lock (_lockObject)
        {
            if (DateTime.Now.Subtract(_lastRequestTime).TotalMilliseconds > _timespanInMilliseconds)
            {
                _requestCount = 0;
                _lastRequestTime = DateTime.Now;
            }

            _requestCount++;

            if (_requestCount > _requestLimit)
            {
                int timeToWait = (int)(_lastRequestTime.AddMilliseconds(_timespanInMilliseconds) - DateTime.Now).TotalMilliseconds;
                if (timeToWait > 0)
                    Thread.Sleep(timeToWait);

                _requestCount = 1;
                _lastRequestTime = DateTime.Now;
            }
        }

        _semaphore.Release();
    }

    public void Dispose()
    {
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
