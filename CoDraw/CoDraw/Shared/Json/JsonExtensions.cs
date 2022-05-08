using System.Text.Json;
using CoDraw.Shared.Events;

namespace CoDraw.Shared.Json;

public static class JsonExtensions
{
    public static JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters =
        {
            new EventConverter<UserEvent, UserEventType>("t")
        },
    };
}