using CoDraw.Shared;

namespace CoDraw.Client.Pages;

public class ClientUpdateBroadcaster : UpdateBroadcaster
{
    private readonly ClientHub _hubContext;
    private readonly UserEventBuilder _userEventBuilder;

    public ClientUpdateBroadcaster(ILogger<UpdateBroadcaster> logger, ClientHub hubContext, IBroadcastTimer timer, UserEventBuilder userEventBuilder) : base(logger, timer)
    {
        _hubContext = hubContext;
        _userEventBuilder = userEventBuilder;
    }
    
    public override async Task Update()
    {
        if (!_hubContext.IsConnected)
        {
            return;
        }

        var userEvents = _userEventBuilder.Build();

        if (userEvents.Events.Count == 0)
        {
            return;
        }

        await _hubContext.SendUpdate(userEvents);
    }
}