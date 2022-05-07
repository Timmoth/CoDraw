namespace CoDraw.Shared;

public class MouseUp : UserEvent, IEquatable<MouseUp>
{
    public MouseUp() : base(UserEventType.MouseUp)
    {
    }

    public bool Equals(MouseUp? other)
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

        return Equals((MouseUp)obj);
    }
}