using System;
using System.Collections.Generic;
using System.Text.Json;
using CoDraw.Shared;
using Xunit;

namespace Tests;

public class SerializationTest
{
    [Fact]
    public void CoDrawLineUpdateEventsSerialization()
    {
        //Given
        var expected = new CoDrawLineUpdateEvents(Guid.NewGuid(), new Dictionary<Guid, List<LineCoDrawEvent>>
        {
            {
                Guid.NewGuid(), new List<LineCoDrawEvent>
                {
                    new LinePointsCoDrawEvent(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), new List<Point>
                    {
                        new(1, 2)
                    })
                }
            }
        });

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new CoDrawEventConverter<LineCoDrawEvent>("type"),
                new CoDrawEventConverter<UserCoDrawEvent>("type")
            }
        };

        //When
        var json = JsonSerializer.Serialize(expected, jsonSerializerOptions);
        var actual = JsonSerializer.Deserialize<CoDrawLineUpdateEvents>(json, jsonSerializerOptions);

        //Then
        Assert.Equal(expected, actual);
    }
}