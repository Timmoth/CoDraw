using CoDraw.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CoDraw.Server;

public class CoDrawHub : Hub
{
    private readonly CoDrawState _coDrawState;

    public CoDrawHub(CoDrawState codrawState, UpdateBroadcaster updateBroadcaster)
    {
        _coDrawState = codrawState;
    }

    public async Task UpdateUserEvents(CoDrawUserUpdateEvents update)
    {
        _coDrawState.Update(update);
    }

    public async Task UpdateLineEvents(CoDrawLineUpdateEvents update)
    {
        _coDrawState.Update(update);
    }
}