using System.Text.Json.Serialization;
using Blazor.Extensions.Canvas.Canvas2D;

namespace CoDraw.Shared.Events;

public abstract class UserEvent
{
    protected UserEvent(UserEventType eventType)
    {
        EventType = eventType;
    }

    [JsonPropertyName("t")] public UserEventType EventType { get; set; }

    public abstract void Apply(UserState state);
    public abstract Task Apply(UserState state, Canvas2DContext context);
}