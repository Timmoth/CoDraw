using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;
using CoDraw.Shared.Events;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace CoDraw.Client.Pages;

partial class Index : ComponentBase, IAsyncDisposable
{
    #region Properties

    [Inject] public Drawer Drawer { get; set; }
    [Inject] public ClientHub CoDrawHub { get; set; }
    [Inject] public BoardState BoardState { get; set; }
    [Inject] public ClientUpdateBroadcaster Updater { get; set; }
    [Inject] public UserEventBuilder UserEventBuilder { get; set; }

    private Canvas2DContext _context;
    protected BECanvasComponent _canvasReference;

    #endregion

    #region Lifecycle

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _context = await _canvasReference.CreateCanvas2DAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    protected override async Task OnInitializedAsync()
    {
        CoDrawHub.OnUpdate += OnUpdate;
        await CoDrawHub.Start();
    }

    private void OnUpdate(object? sender, List<UserEvents> e)
    {
        _ = Update(e);
    }

    private async Task Update(List<UserEvents> userEvents)
    {
        if (_context == null)
        {
            return;
        }

        await Drawer.Draw(_context, userEvents);
    }

    public async ValueTask DisposeAsync()
    {
        await CoDrawHub.DisposeAsync();
    }

    #endregion

    #region Events

    public void MouseDown(MouseEventArgs e)
    {
        var rand = new Random();
        var color = rand.NextDouble() > 0.5 ? "green" : "red";
        UserEventBuilder.StrokeColor(color);
        UserEventBuilder.StrokeThickness((float)rand.NextDouble() * 5);
        UserEventBuilder.MouseDown(new Point((float)e.OffsetX, (float)e.OffsetY));
    }

    public void MouseUp(MouseEventArgs e)
    {
        UserEventBuilder.MouseUp();
    }

    public void MouseOut(MouseEventArgs e)
    {
    }

    public void MouseMove(MouseEventArgs e)
    {
        UserEventBuilder.MouseMove(new Point((float)e.OffsetX, (float)e.OffsetY));
    }

    public void KeyDown(KeyboardEventArgs e)
    {
    }

    public void KeyUp(KeyboardEventArgs e)
    {
    }

    #endregion
}