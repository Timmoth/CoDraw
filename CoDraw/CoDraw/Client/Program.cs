using CoDraw.Client;
using CoDraw.Client.Pages;
using CoDraw.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<IBroadcastTimer, BroadcastTimer>();
builder.Services.AddSingleton<Drawer>();
builder.Services.AddSingleton<ClientHub>();
builder.Services.AddSingleton<BoardState>();
builder.Services.AddSingleton<ClientUpdateBroadcaster>();

await builder.Build().RunAsync();