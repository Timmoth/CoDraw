using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class UserStrokeCoDrawEvent : UserCoDrawEvent
{
    public UserStrokeCoDrawEvent(long time, string strokeStyle) : base(CoDrawEventType.UserStrokeCoDrawEvent, time)
    {
        StrokeStyle = strokeStyle;
    }

    [JsonPropertyName("stroke")] public string StrokeStyle { get; }
}