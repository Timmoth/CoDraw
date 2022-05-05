namespace CoDraw.Shared;

public class DrawingBoard
{
    public Dictionary<Guid, Dictionary<Guid, Line>> UsersLines { get; set; } = new();

    public void Update(Guid userId, Dictionary<Guid, List<LineCoDrawEvent>> updateLineEvents)
    {
        if (!UsersLines.TryGetValue(userId, out var usersLines))
        {
            usersLines = UsersLines[userId] = new Dictionary<Guid, Line>();
        }

        foreach (var (lineId, lineUpdateEvents) in updateLineEvents)
        {
            if (lineUpdateEvents.Count == 0)
            {
                continue;
            }

            if (!usersLines.TryGetValue(lineId, out var lines))
            {
                if (lineUpdateEvents.First() is not LineStartCoDrawEvent startEvent)
                {
                    //If the line has not been created the first update event must be the line created
                    continue;
                }

                lines = usersLines[lineId] = Line.Create(lineId, userId, startEvent.Stroke, startEvent.Points);
            }

            foreach (var lineUpdateEvent in lineUpdateEvents)
            {
                switch (lineUpdateEvent)
                {
                    case LinePointsCoDrawEvent linePointsEvent:
                        lines.Points.AddRange(linePointsEvent.Points);
                        break;
                }
            }
        }
    }
}