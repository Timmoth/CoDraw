using System;
using CoDraw.Server;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Tests;

public class ControllerBuilder<T> where T : Hub
{
    private readonly IConfiguration _configuration;
    private bool _isBuilt;

    public ControllerBuilder(ITestOutputHelper output)
    {
        Services = new ServiceCollection();

        _configuration = new ConfigurationBuilder().Build();

        Services.AddSingleton(_configuration);
        Services.AddLogging(builder => builder.AddXUnit(output));
    }

    public IServiceCollection Services { get; }

    public T Build()
    {
        if (_isBuilt)
        {
            throw new InvalidOperationException("Cannot build more than once.");
        }

        _isBuilt = true;

        Services.AddSingleton<T>();
        Services.AddApiDependenciesThatWorkInTest();

        var sp = Services.BuildServiceProvider();
        var controller = sp.GetRequiredService<T>();
        return controller;
    }

    public ControllerBuilder<T> Add(FakeHubContext<T> hubContext)
    {
        Services.AddSingleton(hubContext.HubContext);
        return this;
    }

    public ControllerBuilder<T> Add(FakeBroadcasterTimer timer)
    {
        Services.AddSingleton(timer.BroadcastTimer);
        return this;
    }
}