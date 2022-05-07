using System.Text.Json;
using CoDraw.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoDraw.Client.Pages;

public class ClientHub : IAsyncDisposable
{
    private readonly HubConnection hubConnection;

    public ClientHub(NavigationManager navigationManager)
    {
        hubConnection = new HubConnectionBuilder().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new EventConverter<UserEvent, UserEventType>("t")
                    }
                };
            })
            .WithUrl(navigationManager.ToAbsoluteUri("/codrawhub"))
            .Build();

        hubConnection.On<List<UserEvents>>("update", ProcessUpdate);
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