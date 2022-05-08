using System.Text.Json.Serialization;
using Blazor.Extensions.Canvas.Canvas2D;

namespace CoDraw.Shared.Events;

public class MouseDown : UserEvent, IEquatable<MouseDown>
{
    public MouseDown(Point point) : base(UserEventType.MouseDown)
    {
        Point = point;
    }

    [JsonPropertyName("p")] public Point Point { get; set; }

    public bool Equals(MouseDown? other)
    {
        return Equals(Point, other.Point);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((MouseDown)obj);
    }

    public override void Apply(UserState state)
    {
        state.MouseDown = true;
        state.Point = Point;
        state.LastLinePoint = null;
    }

    public override async Task Apply(UserState state, Canvas2DContext context)
    {
        await context.SetLineWidthAsync(state.StrokeThickness);
        await context.SetStrokeStyleAsync(state.StrokeColor);
        await context.ArcAsync(Point.X, Point.Y, state.StrokeThickness, 0, 360);
        await context.BeginPathAsync();
    }
}