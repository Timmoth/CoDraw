using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Tests.Mocks;

public class FakeHubContext<T> where T : Hub
{
    public FakeHubContext()
    {
        MockHubContext = new Mock<IHubContext<T>>();
        MockHubClients = new Mock<IHubClients>();
        MockClientProxy_All = new Mock<IClientProxy>();

        MockHubContext.Setup(x => x.Clients).Returns(MockHubClients.Object);

        MockHubClients.Setup(x => x.All).Returns(MockClientProxy_All.Object);

        MockClientProxy_All
            .Setup(x => x.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns<string, object?[], CancellationToken>(
                (a, b, c) =>
                {
                    All_ClientRequests.Add(new SignalRClientRequest(a, b));
                    return Task.CompletedTask;
                });
    }

    public Mock<IHubContext<T>> MockHubContext { get; set; }
    public Mock<IHubClients> MockHubClients { get; set; }
    public Mock<IClientProxy> MockClientProxy_All { get; set; }

    public IHubContext<T> HubContext => MockHubContext.Object;
    public IHubClients HubClients => MockHubClients.Object;

    public List<SignalRClientRequest> All_ClientRequests { get; set; } = new();

    public record SignalRClientRequest(string Method, object?[] Arguments);
}