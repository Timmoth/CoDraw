using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;


namespace CoDraw.Client.Pages
{
    partial class Index : ComponentBase, IAsyncDisposable
    {
        #region Properties
        private Canvas2DContext _context;
        protected BECanvasComponent _canvasReference;

        private readonly Users _users = new();

        private readonly int _lineResolution = 3;
        #endregion

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _context = await _canvasReference.CreateCanvas2DAsync();
            }

            await _context.BeginBatchAsync();
            await _context.SetLineWidthAsync(2);

            foreach (var user in _users.All)
            {
                await _context.SetStrokeStyleAsync(user.StrokeStyle);
                foreach (var line in user.Lines)
                {
                    await _context.BeginPathAsync();
                    foreach (var (x, y) in line.Points)
                    {
                        await _context.LineToAsync(x, y);
                    }

                    await _context.StrokeAsync();
                }
            }
            
            await _context.EndBatchAsync();
        }

        #region Events

        private readonly Guid _userId = Guid.NewGuid();
        private Guid? _lineId = null;
        private Point? _currentPos = null;
        public void MouseDown(MouseEventArgs e)
        {
            _currentPos = new(e.OffsetX, e.OffsetY);
            _lineId = Guid.NewGuid();

            if (hubConnection is not null)
            {
                _ = hubConnection.SendAsync("StartLine", _userId, _lineId, _currentPos);
            }
            _users.AddPoint(_userId, _lineId.Value, _currentPos.Value);
        }

        public void MouseUp(MouseEventArgs e)
        {
            _lineId = null;
            _currentPos = null;

            //if (hubConnection is not null)
            //{
            //    _ = hubConnection.SendAsync("EndLine", _userId);
            //}
        }

        public void MouseOut(MouseEventArgs e)
        {
            //if (hubConnection is not null)
            //{
            //    _ = hubConnection.SendAsync("EndLine", _userId);
            //}

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

            _currentPos = new(e.OffsetX, e.OffsetY);
            if (hubConnection is not null)
            {
                _ = hubConnection.SendAsync("AddLinePoint", _userId, _lineId, _currentPos);
            }

            _users.AddPoint(_userId, _lineId.Value, _currentPos.Value);
        }

        public void KeyDown(KeyboardEventArgs e)
        {

        }

        public void KeyUp(KeyboardEventArgs e)
        {

        }

        #endregion

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }

        private HubConnection? hubConnection;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/codrawhub"))
                .Build();

            hubConnection.On<DrawLinePointEvent>("AddLinePoint", e =>
            {
                if (_userId != e.UserId)
                {
                    _users.AddPoint(e.UserId, e.LineId, e.Point);
                    InvokeAsync(StateHasChanged);
                }
            });

            //hubConnection.On<EndLineEvent>("EndLineEvent", e=>
            //{
            // //   _users.EndLine(e.UserId);
            //    InvokeAsync(StateHasChanged);
            //});

            hubConnection.On<StrokeStyleEvent>("SetStrokeStyle", e =>
            {
                _users.SetStrokeStyle(e.UserId, e.StrokeStyle);
                InvokeAsync(StateHasChanged);
            });

            await hubConnection.StartAsync();

            var color = new Random().NextDouble() > 0.5 ? "green" : "red";
            await hubConnection.SendAsync("SetStyle", _userId, color);
        }

        public bool IsConnected =>
            hubConnection?.State == HubConnectionState.Connected;


    }
}
