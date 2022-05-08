using System.Text.Json.Serialization;
using Blazor.Extensions.Canvas.Canvas2D;

namespace CoDraw.Shared.Events;

public class StrokeColor : UserEvent, IEquatable<StrokeColor>
{
    [JsonPropertyName("color")]
    public string Color { get; set; }
    public StrokeColor(string color) : base(UserEventType.StrokeColor)
    {
        Color = color;
    }

    public bool Equals(StrokeColor? other)
    {
        return Equals(Color, other.Color);
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

        return Equals((StrokeColor)obj);
    }

    public override void Apply(UserState state)
    {
        state.StrokeColor = Color;
    }

    public override async Task Apply(UserState state, Canvas2DContext context)
    {
        await context.SetStrokeStyleAsync(Color);
    }
}