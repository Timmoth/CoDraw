namespace CoDraw.Shared;

public class User
{
    public Guid Id { get; init; }
    public string StrokeStyle { get; set; } = "black";

    public static User Create(Guid id)
    {
        return new User
        {
            Id = id
        };
    }

    public void Update(List<UserCoDrawEvent> updateUserEvents)
    {
        foreach (var update in updateUserEvents.OrderBy(x => x.Time))
        {
            switch (update)
            {
                case UserStrokeCoDrawEvent strokeEvent:
                    StrokeStyle = strokeEvent.StrokeStyle;
                    break;
            }
        }
    }
}