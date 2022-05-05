namespace CoDraw.Shared;

public class CoDrawState
{
    private readonly Dictionary<Guid, User> _users = new();

    private CoDrawUpdate _update = new();
    public DrawingBoard Board { get; init; } = new();
    public IEnumerable<User> All => _users.Select(x => x.Value);

    public CoDrawUpdate? GetUpdate()
    {
        if (_update.IsEmpty)
        {
            return null;
        }

        var update = _update;
        _update = new CoDrawUpdate();
        return update;
    }

    public void Update(CoDrawLineUpdateEvents update)
    {
        if (update.LineEvents.Count <= 0)
        {
            return;
        }

        _update.Update(update);
        Board.Update(update.UserId, update.LineEvents);
    }

    public void Update(CoDrawUserUpdateEvents update)
    {
        if (update.UserEvents.Count <= 0 || !_users.TryGetValue(update.UserId, out var user))
        {
            return;
        }

        _update.Update(update);
        user.Update(update.UserEvents);
    }
}