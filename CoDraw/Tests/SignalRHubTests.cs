using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoDraw.Server;
using CoDraw.Shared;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Tests;

public class SignalRHubTests
{
    private readonly FakeBroadcasterTimer _broadcasterTimer = new();
    private readonly FakeHubContext<ServerHub> _hubContext = new();
    private readonly ITestOutputHelper _output;

    public SignalRHubTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public async Task UserEventsAreDispathcedToSignalRClients()
    {
        //Given
        var userId = Guid.NewGuid();
        var userEvents = new UserEvents(userId, new List<UserEvent>
        {
            new MouseDown(),
            new MouseMove(new List<float> { 0, 0, 1, 1 }),
            new MouseUp()
        });

        var hub = new ControllerBuilder<ServerHub>(_output).Add(_hubContext).Add(_broadcasterTimer).Build();
        var mockClients = new Mock<IHubCallerClients>();
        hub.Clients = mockClients.Object;

        //When
        await hub.Send(userEvents);
        _broadcasterTimer.Tick();

        //Then
        object?[] expected = { userEvents.Events };
        _hubContext.MockClientProxy_All.Verify(
            x => x.SendCoreAsync("update", It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Once);
        var signalRRequest = Assert.Single(_hubContext.SignalRSends);
        var actualUserEvents = Assert.IsType<List<UserEvents>>(signalRRequest.Arguments.Single());
        var eavent = Assert.Single(actualUserEvents);
        Assert.Equal(userEvents, eavent);
    }
}