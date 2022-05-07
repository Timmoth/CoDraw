using CoDraw.Server;
using CoDraw.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBroadcastTimer, BroadcastTimer>();
builder.Services.AddApiDependenciesThatWorkInTest();
builder.Services.AddLogging();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
//builder.Services.AddResponseCompression(opts =>
//{
//    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
//        new[] { "application/octet-stream" });
//});

//SignalR
builder.Services
    .AddSignalR(x =>
    {
        x.EnableDetailedErrors = true;
        x.MaximumReceiveMessageSize = 10240; // bytes
    })
    .AddJsonProtocol(options => { options.PayloadSerializerOptions = JsonExtensions.JsonSerializerOptions; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapHub<ServerHub>("/codrawhub");
app.MapFallbackToFile("index.html");

app.Run();