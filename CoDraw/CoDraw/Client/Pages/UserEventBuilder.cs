using CoDraw.Shared;
using CoDraw.Shared.Events;

namespace CoDraw.Client.Pages;

public class UserEventBuilder
{
    public Guid UserId { get; }
    private readonly float _lineResolution = 1.0f;
    public UserEvents _userEvents { get; private set; }
    private List<float> mousePositions = new();
    private Point _lastPosition { get; set; }

    public UserEventBuilder(Guid userId)
    {
        UserId = userId;
        _userEvents = new(UserId, new List<UserEvent>());
    }

    public UserEventBuilder AddMouseMove()
    {
        if (mousePositions.Count == 0)
        {
            return this;
        }

        var oldMousePositions = mousePositions;
        mousePositions = new List<float>();
        _userEvents.Events.Add(new MouseMove(oldMousePositions));
        return this;
    }

    public UserEvents Build()
    {
        AddMouseMove();
        var update = _userEvents;
        _userEvents = new UserEvents(UserId, new List<UserEvent>());
        return update;
    }

    public UserEventBuilder MouseDown(Point point)
    {
        _userEvents.Events.Add(new MouseDown(point));
        return this;
    } 
    
    public UserEventBuilder StrokeColor(string color)
    {
        _userEvents.Events.Add(new StrokeColor(color));
        return this;
    }    
    public UserEventBuilder StrokeThickness(float thickness)
    {
        _userEvents.Events.Add(new StrokeThickness(thickness));
        return this;
    }

    public UserEventBuilder MouseUp()
    {
        AddMouseMove();
        _userEvents.Events.Add(new MouseUp());
        return this;
    }

    public UserEventBuilder MouseMove(Point position)
    {
        var delta = position - _lastPosition;

        if (
            Math.Abs(delta.X) < _lineResolution &&
            Math.Abs(delta.Y) < _lineResolution)
        {
            return this;
        }

        _lastPosition = position;
        mousePositions.Add(_lastPosition.X);
        mousePositions.Add(_lastPosition.Y);

        return this;
    }
}