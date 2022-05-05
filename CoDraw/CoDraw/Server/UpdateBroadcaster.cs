using CoDraw.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CoDraw.Server;

public class UpdateBroadcaster : IAsyncDisposable
{
    private readonly Timer _broadcastLoop;
    private readonly CoDrawState _coDrawState;
    private readonly IHubContext<CoDrawHub> _hubContext;
    private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);

    public UpdateBroadcaster(IHubContext<CoDrawHub> hubContext, CoDrawState codrawState)
    {
        _coDrawState = codrawState;
        _hubContext = hubContext;

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

    private void TimerTick(object? state)
    {
        var update = _coDrawState.GetUpdate();
        if (update == null)
        {
            return;
        }

        _ = SendUpdate(update);
    }

    private async Task SendUpdate(CoDrawUpdate update)
    {
        await _hubContext.Clients.All.SendAsync("update", update);
    }
}