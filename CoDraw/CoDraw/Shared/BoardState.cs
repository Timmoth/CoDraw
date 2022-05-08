using CoDraw.Shared.Events;

namespace CoDraw.Shared;

public class BoardState
{
    public List<UserEvents> Events { get; } = new();

    public Dictionary<Guid, UserState> Users { get; } = new();

    public List<UserEvents> NewEvents { get; set; } = new();

    public void Add(UserEvents userEvents)
    {
        NewEvents.Add(userEvents);
    }

    public void Clear()
    {
        Events.Clear();
        Users.Clear();
    }

    public List<UserEvents>? Update()
    {
        if (NewEvents.Count == 0)
        {
            return null;
        }

        var orderedEvents = NewEvents;
        NewEvents = new List<UserEvents>();

        foreach (var grouping in orderedEvents)
        {
            var userId = grouping.UserId;
            if (!Users.TryGetValue(userId, out var userState))
            {
                userState = Users[userId] = UserState.Create();
            }

            foreach (var userEvent in grouping.Events)
            {
                userEvent.Apply(userState);
            }
        }

        Events.AddRange(orderedEvents);

        return orderedEvents;
    }
}