namespace CoDraw.Shared;

public class Line
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string StrokeStyle { get; init; }
    public List<Point> Points { get; init; } = new();

    public static Line Create(Guid id, Guid userId, string strokeStyle, List<Point> points)
    {
        return new Line
        {
            Id = id,
            UserId = userId,
            StrokeStyle = strokeStyle,
            Points = points
        };
    }

    public Line Add(List<Point> points)
    {
        Points.AddRange(points);
        return this;
    }
}