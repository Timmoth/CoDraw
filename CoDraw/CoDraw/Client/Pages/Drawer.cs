using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;

namespace CoDraw.Client.Pages;

public class Drawer
{
    private readonly BoardState state;

    public Drawer(BoardState state)
    {
        this.state = state;
    }

    private List<Point> getSplineInterpolationCatmullRom(List<Point> points, int nrOfInterpolatedPoints)
    {
        try
        {
            // The Catmull-Rom Spline, requires at least 4 points so it is possible to extrapolate from 3 points, but not from 2.
            // you would get a straight line anyway
            if (points.Count < 3)
            {
                throw new Exception("Catmull-Rom Spline requires at least 3 points");
            }

            // could throw an error on the following, but it is easily fixed implicitly
            if (nrOfInterpolatedPoints < 1)
            {
                nrOfInterpolatedPoints = 1;
            }

            // create a new pointlist to do splining on
            // if you don't do this, the original pointlist gets extended with the exptrapolated points
            var spoints = points.ToList();

            // always extrapolate the first and last point out
            var dx = spoints[1].X - spoints[0].X;
            var dy = spoints[1].Y - spoints[0].Y;
            spoints.Insert(0, new Point(spoints[0].X - dx, spoints[0].Y - dy));
            dx = spoints[^1].X - spoints[^2].X;
            dy = spoints[^1].Y - spoints[^2].Y;
            spoints.Insert(spoints.Count, new Point(spoints[^1].X + dx, spoints[^1].Y + dy));

            // Note the nrOfInterpolatedPoints acts as a kind of tension factor between 0 and 1 because it is normalised
            // to 1/nrOfInterpolatedPoints. It can never be 0
            float t = 0;
            var spline = new List<Point>();
            var loopTo = spoints.Count - 4;
            for (var i = 0; i <= loopTo; i++)
            {
                var loopTo1 = nrOfInterpolatedPoints - 1;
                for (var intp = 0; intp <= loopTo1; intp++)
                {
                    t = 1 / (float)nrOfInterpolatedPoints * intp;

                    var spoint = 0.5f * (2 * spoints[i + 1] + (-1 * spoints[i] + spoints[i + 2]) * t +
                                         (2 * spoints[i] - 5 * spoints[i + 1] + 4 * spoints[i + 2] - spoints[i + 3]) *
                                         (float)Math.Pow(t, 2) +
                                         (-1 * spoints[i] + 3 * spoints[i + 1] - 3 * spoints[i + 2] + spoints[i + 3]) *
                                         (float)Math.Pow(t, 3));
                    spline.Add(spoint);
                }
            }

            // add the last point, but skip the interpolated last point, so second last...
            spline.Add(spoints[^2]);
            return spline;
        }
        catch (Exception exc)
        {
            // Debug.Print(exc.ToString)
            return null;
        }
    }

    public async Task Draw(Canvas2DContext _context, List<UserEvents> e)
    {
        await _context.BeginBatchAsync();

        foreach (var grouping in e)
        {
            if (!state.Users.TryGetValue(grouping.UserId, out var userState))
            {
                userState = state.Users[grouping.UserId] = UserState.Create();
            }

            await Draw(userState, grouping.Events, _context);
        }

        await _context.EndBatchAsync();
    }

    public async Task<bool> Apply(UserState userState, UserEvent userEvent, Canvas2DContext context)
    {
        switch (userEvent)
        {
            case MouseDown mouseDown:
                await BeginDraw(userState, context);
                return true;
            case MouseMove mouseMove:
                var points = mouseMove.GetPoints();

                if (points.Count == 0)
                {
                    return false;
                }

                var lastPoint = userState.Point;

                if (userState.MouseDown)
                {
                    points.Insert(0, lastPoint);

                    //if (points.Count >= 3)
                    //{
                    //    points = getSplineInterpolationCatmullRom(points, 20);
                    //}

                    //await context.ArcAsync(lastPoint.X, lastPoint.Y, 10.0, 0, 360);

                    foreach (var point in points)
                    {
                        await context.LineToAsync(point.X, point.Y);
                        await context.ArcAsync(point.X, point.Y, 5.0, 0, 360);
                    }
                }

                lastPoint = points.Last();
                userState.Point = new Point(lastPoint.X, lastPoint.Y);

                return true;
            case MouseUp mouseUp:
                await context.StrokeAsync();
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public async Task Draw(UserState userState, List<UserEvent> userEvents, Canvas2DContext context)
    {
        await context.BeginBatchAsync();

        var hasDrawn = false;
        if (userState.MouseDown)
        {
            await BeginDraw(userState, context);
            hasDrawn = true;
        }

        foreach (var userEvent in userEvents)
        {
            hasDrawn = await Apply(userState, userEvent, context) || hasDrawn;
            userState.Apply(userEvent);
        }

        if (hasDrawn)
        {
            await context.StrokeAsync();
        }

        await context.EndBatchAsync();
    }

    private async Task BeginDraw(UserState userState, Canvas2DContext context)
    {
        await context.SetLineWidthAsync(2);
        var color = new Random().NextDouble() > 0.5 ? "green" : "red";
        await context.SetStrokeStyleAsync(color);
        await context.BeginPathAsync();
    }
}