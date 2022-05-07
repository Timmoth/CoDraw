using System;
using CoDraw.Shared;
using Moq;

namespace Tests;

public class FakeBroadcasterTimer
{
    public FakeBroadcasterTimer()
    {
        MockBroadcastTimer = new Mock<IBroadcastTimer>();
    }

    public Mock<IBroadcastTimer> MockBroadcastTimer { get; set; }

    public IBroadcastTimer BroadcastTimer => MockBroadcastTimer.Object;

    public void Tick()
    {
        MockBroadcastTimer.Raise(x => x.OnTick += null, this, EventArgs.Empty);
    }
}