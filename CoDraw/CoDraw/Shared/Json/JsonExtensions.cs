using System.Text.Json;

namespace CoDraw.Shared;

public static class JsonExtensions
{
    public static JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters =
        {
            new EventConverter<UserEvent, UserEventType>("t")
        }
    };
}