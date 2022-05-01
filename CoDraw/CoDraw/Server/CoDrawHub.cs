using CoDraw.Shared;
using Microsoft.AspNetCore.SignalR;

namespace CoDraw.Server
{
    public class CoDrawHub : Hub
    {
        private readonly Users _users = new();
        public async Task StartLine(Guid userId, Guid lineId, Point point)
        {
            var line = _users.StartLine(userId, lineId, point);
            await Clients.All.SendAsync("AddLinePoint", new DrawLinePointEvent(userId, line.Id, point));
        }

        public async Task EndLine(Guid userId, Guid lineId)
        {
            var line = _users.EndLine(userId, lineId);
            if (line != null)
            {
                await Clients.All.SendAsync("EndLine", new EndLineEvent(userId, line.Id));
            }
        }

        public async Task AddLinePoint(Guid userId, Guid lineId, Point point)
        {
            var line = _users.AddPoint(userId, lineId, point); 
            await Clients.All.SendAsync("AddLinePoint",  new DrawLinePointEvent(userId, lineId, point));
        }

        public async Task SetStyle(Guid userId, string strokeStyle)
        {
            _users.SetStrokeStyle(userId, strokeStyle);

            await Clients.All.SendAsync("SetStrokeStyle", new StrokeStyleEvent(userId, strokeStyle));
        }
    }
}
