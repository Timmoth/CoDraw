using CoDraw.Shared;

namespace CoDraw.Client.Pages;

public class ClientUpdateBroadcaster : UpdateBroadcaster
{
    private static readonly Guid _userId = Guid.NewGuid();
    private readonly BoardState _coDrawState;
    private readonly ClientHub _hubContext;
    private readonly float _lineResolution = 1.0f;
    private UserEvents Events = new(_userId, new List<UserEvent>());
    private List<float> points = new();

    public ClientUpdateBroadcaster(ILogger<UpdateBroadcaster> logger, ClientHub hubContext, BoardState codrawState,
        IBroadcastTimer timer) : base(logger, timer)
    {
        _coDrawState = codrawState;
        _hubContext = hubContext;
    }

    private Point point { get; set; }

    public void AddMouseMove()
    {
        if (points.Count == 0)
        {
            return;
        }

        var old = points;
        points = new List<float>();
        Events.Events.Add(new MouseMove(old));
    }

    public override async Task Update()
    {
        if (!_hubContext.IsConnected)
        {
            return;
        }

        AddMouseMove();

        if (Events.Events.Count == 0)
        {
            return;
        }

        var update = Events;
        Events = new UserEvents(_userId, new List<UserEvent>());


        _logger.LogInformation("Updater ran in: '{duration}'",
            update.Events.Sum(x => x is MouseMove m ? m.Points.Count : 0));

        await _hubContext.SendUpdate(update);
    }

    public void MouseDown()
    {
        Events.Events.Add(new MouseDown());
    }

    public void MouseUp()
    {
        AddMouseMove();
        Events.Events.Add(new MouseUp());
    }

    public void MouseMove(Point p)
    {
        if (
            Math.Abs(p.X - point.X) < _lineResolution &&
            Math.Abs(p.Y - point.Y) < _lineResolution)
        {
            return;
        }

        point = p;
        points.Add(point.X);
        points.Add(point.Y);
    }
}