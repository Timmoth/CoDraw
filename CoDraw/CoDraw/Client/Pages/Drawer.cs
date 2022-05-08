using Blazor.Extensions.Canvas.Canvas2D;
using CoDraw.Shared;
using CoDraw.Shared.Events;

namespace CoDraw.Client.Pages;

public class Drawer
{
    private readonly BoardState state;

    public Drawer(BoardState state)
    {
        this.state = state;
    }

    public async Task Draw(Canvas2DContext _context, List<UserEvents> e)
    {
        await _context.BeginBatchAsync();

        foreach (var grouping in e)
        {
            if (!state.Users.TryGetValue(grouping.UserId, out var userState))
            {
                userState = state.Users[grouping.UserId] = UserState.Create();
            }

            await Draw(userState, grouping.Events, _context);
        }

        await _context.EndBatchAsync();
    }

    private async Task Draw(UserState userState, List<UserEvent> userEvents, Canvas2DContext context)
    {
        if (userState.MouseDown)
        {
            await context.SetLineWidthAsync(userState.StrokeThickness);
            await context.SetStrokeStyleAsync(userState.StrokeColor);
            await context.BeginPathAsync();
        }

        foreach (var userEvent in userEvents)
        {
            await userEvent.Apply(userState, context);
            userEvent.Apply(userState);
        }

        if (userState.MouseDown)
        {
            await context.StrokeAsync();
        }
    }
}