namespace CoDraw.Shared;

public class MouseDown : UserEvent, IEquatable<MouseDown>
{
    public MouseDown() : base(UserEventType.MouseDown)
    {
    }

    public bool Equals(MouseDown? other)
    {
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((MouseDown)obj);
    }
}