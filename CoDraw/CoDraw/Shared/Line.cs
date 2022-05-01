using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoDraw.Shared
{
    public record struct DrawLinePointEvent(Guid UserId, Guid LineId, Point Point);
    public record struct EndLineEvent(Guid UserId, Guid LineId);
    public record struct StrokeStyleEvent(Guid UserId, string StrokeStyle);

    public record struct Point(double X, double Y);
    public class Line
    {
        public Guid Id { get; init; }
        public List<Point> Points { get; } = new();

        public static Line Create(Guid id, Point point)
        {
            return new Line()
            {
                Id = id,
                Points = { point }
            };
        }

        public Line Add(Point point)
        {
            Points.Add(point);
            return this;
        }
    }

    public class User
    {
        public Guid Id { get; init; }
        public List<Line> Lines { get; } = new();
        public Line? CurrentLine { get; private set; }
        public string StrokeStyle { get; set; }= "black";

        public static User Create(Guid id)
        {
            return new User()
            {
                Id = id
            };
        }

        public Line StartLine(Guid id, Point point)
        {
            CurrentLine = Line.Create(id, point);
            Lines.Add(CurrentLine);
            return CurrentLine;
        }

        public Line? EndLine()
        {
            var endedLine = CurrentLine;
            CurrentLine = null;
            return endedLine;
        }
    }
    public class Users
    {
        private readonly Dictionary<Guid, User> _users = new();

        public IEnumerable<User> All => _users.Select(x => x.Value);

        public Line StartLine(Guid userId, Guid lineId, Point point)
        {
            if (!_users.TryGetValue(userId, out var userLines))
            {
                userLines = _users[userId] = User.Create(userId);
            }

            return userLines.StartLine(lineId, point);
        }

        public Line? EndLine(Guid userId, Guid lineId)
        {
            if (!_users.TryGetValue(userId, out var userLines))
            {
                userLines = _users[userId] = User.Create(userId);
            }
            var line = userLines.Lines.Single(x => x.Id == lineId);
            return line;
        }

        public Line AddPoint(Guid userId, Guid lineId, Point point)
        {
            if (!_users.TryGetValue(userId, out var userLines))
            {
                userLines = _users[userId] = User.Create(userId);
            }

            var line = userLines.Lines.FirstOrDefault(x => x.Id == lineId);
            if (line == default)
            {
                return userLines.StartLine(lineId, point);
            }

            line.Add(point);
            return line;
        }

        public User? SetStrokeStyle(Guid userId, string strokeStyle)
        {
            if (!_users.TryGetValue(userId, out var userLines))
            {
                userLines = _users[userId] = User.Create(userId);
            }

            userLines.StrokeStyle = strokeStyle;
            return userLines;
        }
    }
}
