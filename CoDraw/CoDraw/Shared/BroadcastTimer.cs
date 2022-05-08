namespace CoDraw.Shared;

public class BroadcastTimer : IBroadcastTimer, IAsyncDisposable
{
    private readonly Timer _broadcastLoop;

    private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(10);

    public BroadcastTimer()
    {
        _broadcastLoop = new Timer(
            TimerTick,
            null,
            BroadcastInterval,
            BroadcastInterval);
    }

    public async ValueTask DisposeAsync()
    {
        await _broadcastLoop.DisposeAsync();
    }

    public event EventHandler? OnTick;

    private void TimerTick(object? state)
    {
        OnTick?.Invoke(this, EventArgs.Empty);
    }
}