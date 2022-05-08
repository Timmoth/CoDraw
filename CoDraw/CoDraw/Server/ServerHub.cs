using CoDraw.Shared;
using CoDraw.Shared.Events;
using Microsoft.AspNetCore.SignalR;

namespace CoDraw.Server;

public class ServerHub : Hub
{
    private readonly BoardState _boardState;
    private readonly ServerUpdateBroadcaster _updateBroadcaster;

    public ServerHub(ServerUpdateBroadcaster updateBroadcaster, BoardState boardState)
    {
        _updateBroadcaster = updateBroadcaster;
        _boardState = boardState;
    }

    public async Task Send(UserEvents events)
    {
        _boardState.Add(events);
    }

    public async Task<List<UserEvents>> GetState()
    {
        return _boardState.Events;
    }
}