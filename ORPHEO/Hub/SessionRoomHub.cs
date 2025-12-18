using Microsoft.AspNetCore.SignalR;

namespace Orpheo.Hubs
{
    public class SessionRoomHub : Hub
    {
        public async Task JoinRoom(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
        }

        public async Task PlaySong(int roomId, int songIndex, double currentTime)
        {
            await Clients
                .Group($"room-{roomId}")
                .SendAsync("PlaySong", songIndex, currentTime);
        }

        public async Task PauseSong(int roomId, double currentTime)
        {
            await Clients
                .Group($"room-{roomId}")
                .SendAsync("PauseSong", currentTime);
        }
        public async Task SeekSong(int roomId, double currentTime)
        {
            await Clients
                .Group($"room-{roomId}")
                .SendAsync("SeekSong", currentTime);
        }

    }
}
