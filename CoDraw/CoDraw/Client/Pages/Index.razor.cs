using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;
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
        Updater.MouseDown();
    }

    public void MouseUp(MouseEventArgs e)
    {
        Updater.MouseUp();
    }

    public void MouseOut(MouseEventArgs e)
    {
    }

    public void MouseMove(MouseEventArgs e)
    {
        //_ = _context.ArcAsync((float)e.OffsetX, (float)e.OffsetY, 5.0, 0, 360);

        Updater.MouseMove(new Point((float)e.OffsetX, (float)e.OffsetY));
    }

    public void KeyDown(KeyboardEventArgs e)
    {
    }

    public void KeyUp(KeyboardEventArgs e)
    {
    }

    #endregion
}