using CoDraw.Shared;

namespace CoDraw.Client.Pages;

public class BoardConfig
{
    private readonly UserEventBuilder _eventBuilder;


    private string _strokeColor = "black";

    private float _strokeThickness = 2;

    public BoardConfig(UserEventBuilder eventBuilder)
    {
        _eventBuilder = eventBuilder;
    }

    public float StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            _strokeThickness = value;
            _eventBuilder.StrokeThickness(value);
        }
    }

    public string StrokeColor
    {
        get => _strokeColor;
        set
        {
            _strokeColor = value;
            _eventBuilder.StrokeColor(value);
        }
    }

    public static BoardConfig FromUserState(UserState state, UserEventBuilder eventBuilder)
    {
        return new BoardConfig(eventBuilder)
        {
            _strokeColor = state.StrokeColor,
            _strokeThickness = state.StrokeThickness
        };
    }
}