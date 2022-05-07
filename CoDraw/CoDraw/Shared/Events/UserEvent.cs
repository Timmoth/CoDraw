using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public abstract class UserEvent
{
    protected UserEvent(UserEventType eventType)
    {
        EventType = eventType;
    }

    [JsonPropertyName("t")] public UserEventType EventType { get; set; }
}