using CoDraw.Shared;
using CoDraw.Shared.Events;
using CoDraw.Shared.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoDraw.Client.Pages;

public class ClientHub : IAsyncDisposable
{
    private readonly BoardState _state;
    private readonly HubConnection hubConnection;

    public ClientHub(NavigationManager navigationManager, BoardState state)
    {
        _state = state;
        hubConnection = new HubConnectionBuilder().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = JsonExtensions.JsonSerializerOptions;
            })
            .WithAutomaticReconnect()
            .WithUrl(navigationManager.ToAbsoluteUri("/codrawhub"))
            .Build();

        hubConnection.On<List<UserEvents>>("Update", ProcessUpdate);
        hubConnection.Reconnected += HubConnectionOnReconnected;
    }

    private async Task HubConnectionOnReconnected(string? arg)
    {
        await Reload();
    }

    public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;
    public EventHandler<List<UserEvents>> OnUpdate { get; set; }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }

    public async Task Start()
    {
        await hubConnection.StartAsync();
        await Reload();
    }

    private async Task Reload()
    {
        _state.Clear();
        if (IsConnected)
        {
            var userEvents = await hubConnection.InvokeCoreAsync<List<UserEvents>>("GetState", new object?[] { });
            ProcessUpdate(userEvents);
        }
    }

    private void ProcessUpdate(List<UserEvents> update)
    {
        OnUpdate?.Invoke(this, update);
    }

    public async Task SendUpdate(UserEvents update)
    {
        if (!IsConnected)
        {
            return;
        }

        await hubConnection.SendAsync("Send", update);
    }
}