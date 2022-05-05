namespace CoDraw.Shared;

public abstract class UserCoDrawEvent : CoDrawEvent
{
    protected UserCoDrawEvent(CoDrawEventType typeDiscriminator, long time) : base(typeDiscriminator, time)
    {
    }
}