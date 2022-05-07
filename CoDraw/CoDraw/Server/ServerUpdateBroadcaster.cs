using CoDraw.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CoDraw.Server;

public class ServerUpdateBroadcaster : UpdateBroadcaster
{
    private readonly BoardState _coDrawState;
    private readonly IHubContext<ServerHub> _hubContext;

    public ServerUpdateBroadcaster(ILogger<UpdateBroadcaster> logger, IHubContext<ServerHub> hubContext,
        BoardState codrawState, IBroadcastTimer timer) : base(logger, timer)
    {
        _coDrawState = codrawState;
        _hubContext = hubContext;
    }

    public override async Task Update()
    {
        var update = _coDrawState.Update();
        if (update == null)
        {
            return;
        }

        await _hubContext.Clients.All.SendCoreAsync("update", new[] { update });
    }
}