namespace CoDraw.Shared;

public class UserState
{
    public bool MouseDown { get; set; }
    public Point? LastLinePoint { get; set; }
    public Point Point { get; set; }
    public float StrokeThickness { get; set; }
    public string StrokeColor { get; set; }

    public static UserState Create()
    {
        return new UserState
        {
            MouseDown = false,
            Point = new Point(0, 0),
            StrokeThickness = 2,
            StrokeColor = "black"
        };
    }
}