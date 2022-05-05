﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class CoDrawEventConverter<T> : JsonConverter<T>
{
    private readonly string _discriminator;
    private readonly IEnumerable<Type> _types;

    public CoDrawEventConverter(string discriminator)
    {
        _discriminator = discriminator;
        var type = typeof(T);
        _types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && p.IsClass)
            .ToList();
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        using var jsonDocument = JsonDocument.ParseValue(ref reader);
        if (!jsonDocument.RootElement.TryGetProperty(_discriminator, out var typeProperty))
        {
            throw new JsonException();
        }

        var eventType = (CoDrawEventType)typeProperty.GetInt16();
        var type = _types.FirstOrDefault(x => x.Name == eventType.ToString());
        if (type == null)
        {
            throw new JsonException();
        }

        var jsonObject = jsonDocument.RootElement.GetRawText();
        var result = (T)JsonSerializer.Deserialize(jsonObject, type, options)!;

        return result;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, options);
    }
}