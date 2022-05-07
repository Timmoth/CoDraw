using CoDraw.Shared;

namespace CoDraw.Server;

public static class StartupExtensions
{
    public static void AddApiDependenciesThatWorkInTest(this IServiceCollection services)
    {
        services.AddSingleton<ServerUpdateBroadcaster>();
        services.AddSingleton<BoardState>();
    }
}