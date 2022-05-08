using System;
using System.Text.Json;
using CoDraw.Client.Pages;
using CoDraw.Shared;
using CoDraw.Shared.Events;
using CoDraw.Shared.Json;
using Xunit;

namespace Tests;

public class SerializationTest
{
    [Fact]
    public void UserEventsEventsSerialization()
    {
        //Given
        var expected = new UserEventBuilder(Guid.NewGuid())
            .StrokeColor("green")
            .StrokeThickness(0.5f)
            .MouseDown(new Point(5, 5))
            .MouseMove(new Point(10, 10))
            .MouseMove(new Point(20, 10))
            .MouseUp()
            .Build();

        //When
        var json = JsonSerializer.Serialize(expected, JsonExtensions.JsonSerializerOptions);
        var actual = JsonSerializer.Deserialize<UserEvents>(json, JsonExtensions.JsonSerializerOptions);

        //Then
        Assert.Equal(expected, actual);
    }
}