using System.Text.Json;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;

namespace CoDraw.Client.Pages;

partial class Index : ComponentBase, IAsyncDisposable
{
    [Inject] public NavigationManager NavigationManager { get; set; }


    public void TimerTick(object state)
    {
        if (hubConnection is not { State: HubConnectionState.Connected })
        {
            return;
        }

        var update = lineUpdate;
        if (update.LineEvents.Count == 0)
        {
            return;
        }

        lineUpdate = new CoDrawLineUpdateEvents(_userId, new Dictionary<Guid, List<LineCoDrawEvent>>());
        _ = hubConnection.SendAsync("UpdateLineEvents", update);
    }

    private async Task Update(CoDrawUpdate? e)
    {
        foreach (var (userId, userUpdates) in e.UserEvents)
        {
            _coDrawState.Update(new CoDrawUserUpdateEvents(userId, userUpdates));
        }

        foreach (var (lineId, lineUpdates) in e.LineEvents)
        {
            _coDrawState.Update(new CoDrawLineUpdateEvents(lineId, lineUpdates));
        }

        await Draw();
    }

    private async Task Draw()
    {
        if (_context == null)
        {
            return;
        }

        await _context.BeginBatchAsync();
        await _context.SetLineWidthAsync(2);

        foreach (var (userId, lines) in _coDrawState.Board.UsersLines)
        {
            foreach (var (lineId, line) in lines)
            {
                await _context.SetStrokeStyleAsync(line.StrokeStyle);
                await _context.BeginPathAsync();
                foreach (var (x, y) in line.Points)
                {
                    await _context.LineToAsync(x, y);
                }

                await _context.StrokeAsync();
            }
        }

        await _context.EndBatchAsync();
        await InvokeAsync(StateHasChanged);
    }

    #region Properties

    private Canvas2DContext _context;
    protected BECanvasComponent _canvasReference;

    private readonly CoDrawState _coDrawState = new();

    private readonly int _lineResolution = 2;
    private Timer timer;
    private readonly TimeSpan BroadcastInterval = TimeSpan.FromMilliseconds(40);
    private static readonly Guid _userId = Guid.NewGuid();
    private Guid? _lineId;
    private Point? _currentPos;

    private CoDrawLineUpdateEvents lineUpdate = new(_userId, new Dictionary<Guid, List<LineCoDrawEvent>>());
    private HubConnection? hubConnection;
    public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

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
        timer = new Timer(
            TimerTick,
            null,
            BroadcastInterval,
            BroadcastInterval);

        hubConnection = new HubConnectionBuilder().AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions = new JsonSerializerOptions
                {
                    Converters =
                    {
                        new CoDrawEventConverter<LineCoDrawEvent>("type"),
                        new CoDrawEventConverter<UserCoDrawEvent>("type")
                    }
                };
            })
            .WithUrl(NavigationManager.ToAbsoluteUri("/codrawhub"))
            .Build();


        hubConnection.On<CoDrawUpdate?>("update", e => { _ = Update(e); });

        await hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }

        await timer.DisposeAsync();
    }

    #endregion

    #region Events

    public void MouseDown(MouseEventArgs e)
    {
        _currentPos = new Point(e.OffsetX, e.OffsetY);
        _lineId = Guid.NewGuid();
        var color = new Random().NextDouble() > 0.5 ? "green" : "red";

        lineUpdate.Add(_lineId.Value,
            new LineStartCoDrawEvent(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), color,
                new List<Point> { _currentPos.Value }));
    }

    public void MouseUp(MouseEventArgs e)
    {
        _lineId = null;
        _currentPos = null;
    }

    public void MouseOut(MouseEventArgs e)
    {
        _lineId = null;
        _currentPos = null;
    }

    public void MouseMove(MouseEventArgs e)
    {
        if (!_currentPos.HasValue ||
            !_lineId.HasValue ||
            Math.Abs(e.OffsetX - _currentPos.Value.X) < _lineResolution ||
            Math.Abs(e.OffsetY - _currentPos.Value.Y) < _lineResolution)
        {
            return;
        }

        _currentPos = new Point(e.OffsetX, e.OffsetY);
        if (hubConnection is not null)
        {
            lineUpdate.Add(_lineId.Value,
                new LinePointsCoDrawEvent(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    new List<Point> { _currentPos.Value }));
        }
    }

    public void KeyDown(KeyboardEventArgs e)
    {
    }

    public void KeyUp(KeyboardEventArgs e)
    {
    }

    #endregion
}