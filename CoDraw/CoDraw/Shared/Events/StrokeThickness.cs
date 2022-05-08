using System.Text.Json.Serialization;
using Blazor.Extensions.Canvas.Canvas2D;

namespace CoDraw.Shared.Events;

public class StrokeThickness : UserEvent, IEquatable<StrokeThickness>
{
    public StrokeThickness(float thickness) : base(UserEventType.StrokeThickness)
    {
        Thickness = thickness;
    }

    [JsonPropertyName("thickness")] public float Thickness { get; set; }

    public bool Equals(StrokeThickness? other)
    {
        return Equals(Thickness, other.Thickness);
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

        return Equals((StrokeThickness)obj);
    }

    public override void Apply(UserState state)
    {
        state.StrokeThickness = Thickness;
    }

    public override async Task Apply(UserState state, Canvas2DContext context)
    {
        await context.SetLineWidthAsync(Thickness);
    }
}