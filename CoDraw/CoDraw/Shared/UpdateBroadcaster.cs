using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CoDraw.Shared;

public interface IBroadcastTimer
{
    event EventHandler OnTick;
}

public abstract class UpdateBroadcaster
{
    protected readonly ILogger<UpdateBroadcaster> _logger;
    private readonly IBroadcastTimer _timer;
    private bool _updateRunning;

    protected UpdateBroadcaster(ILogger<UpdateBroadcaster> logger, IBroadcastTimer timer)
    {
        _logger = logger;
        _timer = timer;
        _timer.OnTick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        _ = RunUpdate();
    }

    private async Task RunUpdate()
    {
        if (_updateRunning)
        {
            return;
        }

        _updateRunning = true;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        await Update();
        _updateRunning = false;
        stopwatch.Stop();
        _logger.LogInformation("Updater ran in: '{duration}'", stopwatch.Elapsed);
    }

    public abstract Task Update();
}