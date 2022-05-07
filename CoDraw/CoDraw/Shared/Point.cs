namespace CoDraw.Shared;

public record struct Point(float X, float Y)
{
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y);
    }

    public static Point operator +(Point p, float d)
    {
        return new Point(p.X + d, p.Y + d);
    }

    public static Point operator +(float d, Point p)
    {
        return p + d;
    }

    public static Point operator -(Point p1, Point p2)
    {
        return new Point(p1.X - p2.X, p1.Y - p2.Y);
    }

    public static Point operator -(Point p, float d)
    {
        return new Point(p.X - d, p.Y - d);
    }

    public static Point operator -(float d, Point p)
    {
        return p - d;
    }

    public static Point operator *(Point p1, Point p2)
    {
        return new Point(p1.X * p2.X, p1.Y * p2.Y);
    }

    public static Point operator *(Point p, float d)
    {
        return new Point(p.X * d, p.Y * d);
    }

    public static Point operator *(float d, Point p)
    {
        return p * d;
    }

    public static Point operator /(Point p1, Point p2)
    {
        return new Point(p1.X / p2.X, p1.Y / p2.Y);
    }

    public static Point operator /(Point p, float d)
    {
        return new Point(p.X / d, p.Y / d);
    }

    public static Point operator /(float d, Point p)
    {
        return new Point(d / p.X, d / p.Y);
    }
}