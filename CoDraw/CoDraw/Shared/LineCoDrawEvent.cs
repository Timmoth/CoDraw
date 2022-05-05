namespace CoDraw.Shared;

public abstract class LineCoDrawEvent : CoDrawEvent
{
    protected LineCoDrawEvent(CoDrawEventType typeDiscriminator, long time) : base(typeDiscriminator, time)
    {
    }
}