using System.Text.Json.Serialization;

namespace CoDraw.Shared;

public class CoDrawUpdate
{
    [JsonIgnore] public bool IsEmpty => LineEvents.Count == 0 && UserEvents.Count == 0;

    [JsonPropertyName("userevents")] public Dictionary<Guid, List<UserCoDrawEvent>> UserEvents { get; set; } = new();

    [JsonPropertyName("lineevents")]
    public Dictionary<Guid, Dictionary<Guid, List<LineCoDrawEvent>>> LineEvents { get; set; } = new();

    public void Update(CoDrawLineUpdateEvents update)
    {
        if (!LineEvents.TryGetValue(update.UserId, out var usersLineEvents))
        {
            usersLineEvents = LineEvents[update.UserId] = new Dictionary<Guid, List<LineCoDrawEvent>>();
        }

        foreach (var (lineId, lineUpdate) in update.LineEvents)
        {
            if (!usersLineEvents.TryGetValue(lineId, out var lineEvents))
            {
                lineEvents = usersLineEvents[lineId] = new List<LineCoDrawEvent>();
            }

            lineEvents.AddRange(lineUpdate);
        }
    }

    public void Update(CoDrawUserUpdateEvents update)
    {
        if (update.UserEvents.Count <= 0)
        {
            return;
        }

        if (!UserEvents.TryGetValue(update.UserId, out var user))
        {
            user = UserEvents[update.UserId] = new List<UserCoDrawEvent>();
        }

        user.AddRange(update.UserEvents);
    }
}