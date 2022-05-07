namespace CoDraw.Shared;

public class UserState
{
    public bool MouseDown { get; set; }
    public Point Point { get; set; }

    public static UserState Create()
    {
        return new UserState
        {
            MouseDown = false,
            Point = new Point(0, 0)
        };
    }

    public void Apply(UserEvent userEvent)
    {
        switch (userEvent)
        {
            case MouseDown mouseDown:
                MouseDown = true;
                break;
            case MouseMove mouseMove:
                var points = mouseMove.GetPoints();

                if (points.Count == 0)
                {
                    break;
                }

                var last = points.Last();
                Point = new Point(last.X, last.Y);
                break;
            case MouseUp mouseUp:
                MouseDown = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}