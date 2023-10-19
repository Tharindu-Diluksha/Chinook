using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface ITrackService
    {
        Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string currentUserId);
    }
}
