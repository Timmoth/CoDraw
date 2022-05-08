using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoDraw.Client.Pages;
using CoDraw.Server;
using CoDraw.Shared;
using CoDraw.Shared.Events;
using Moq;
using Tests.Mocks;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class SignalRHubTests
{
    private readonly FakeBroadcasterTimer _broadcasterTimer = new();
    private readonly FakeHubContext<ServerHub> _hubContext = new();
    private readonly ITestOutputHelper _output;

    private readonly UserEvents userAEvents;
    private readonly UserEvents userBEvents; 
    public SignalRHubTests(ITestOutputHelper output)
    {
        _output = output;

        userAEvents = new UserEventBuilder(Guid.NewGuid())
            .MouseDown(new Point(15, 5))
            .MouseMove(new Point(10, 10))
            .MouseMove(new Point(20, 10))
            .MouseUp()
            .Build();

        userBEvents = new UserEventBuilder(Guid.NewGuid())
            .MouseMove(new Point(35, 40))
            .MouseUp()
            .StrokeColor("green")
            .StrokeThickness(0.5f)
            .Build();

    }

    [Fact]
    public async Task SignalRClientsGetUpdatedOnTimerTick()
    {
        //Given
        var hub = CreateHub();

        //When
        await hub.Send(userAEvents);
        await hub.Send(userBEvents);
        _broadcasterTimer.Tick();

        //Then
        _hubContext.MockClientProxy_All.Verify(
            x => x.SendCoreAsync("Update", It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Once);
        var clientUpdate = Assert.Single(_hubContext.All_ClientRequests);
        var clientUpdatePayload = Assert.Single(clientUpdate.Arguments);
        var clientUpdateUserEvents = Assert.IsType<List<UserEvents>>(clientUpdatePayload);

        Assert.Equal(2, clientUpdateUserEvents.Count);
        Assert.Contains(clientUpdateUserEvents, x => x.Equals(userAEvents));
        Assert.Contains(clientUpdateUserEvents, x => x.Equals(userBEvents));
    }

    [Fact]
    public async Task GetStateReturnsCurrentBoardState()
    {
        //Given
        var hub = CreateHub();
        await hub.Send(userAEvents);
        await hub.Send(userBEvents);
        _broadcasterTimer.Tick();

        //When
        var userEvents = await hub.GetState();

        //Then
        Assert.Equal(2, userEvents.Count);
        Assert.Contains(userEvents, x => x.Equals(userAEvents));
        Assert.Contains(userEvents, x => x.Equals(userBEvents));
    }

    private ServerHub CreateHub()
    {
        return new ControllerBuilder<ServerHub>(_output).Add(_hubContext).Add(_broadcasterTimer).Build();
    }
}