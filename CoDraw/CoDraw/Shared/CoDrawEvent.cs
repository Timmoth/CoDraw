using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public abstract class CoDrawEvent
{
    protected CoDrawEvent(CoDrawEventType typeDiscriminator, long time)
    {
        TypeDiscriminator = typeDiscriminator;
        Time = time;
    }

    [JsonPropertyName("type")] public CoDrawEventType TypeDiscriminator { get; set; }

    [JsonPropertyName("time")] public long Time { get; set; }
}