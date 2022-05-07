using System;
using System.Collections.Generic;
using System.Text.Json;
using CoDraw.Shared;
using Xunit;

namespace Tests;

public class SerializationTest
{
    [Fact]
    public void UserEventsEventsSerialization()
    {
        //Given
        var expected = new UserEvents(Guid.NewGuid(), new List<UserEvent>
        {
            new MouseDown(),
            new MouseMove(new List<float> { 0, 0, 1, 1, 1, 3 }),
            new MouseUp()
        });

        //When
        var json = JsonSerializer.Serialize(expected, JsonExtensions.JsonSerializerOptions);
        var actual = JsonSerializer.Deserialize<UserEvents>(json, JsonExtensions.JsonSerializerOptions);

        //Then
        Assert.Equal(expected, actual);
    }
}