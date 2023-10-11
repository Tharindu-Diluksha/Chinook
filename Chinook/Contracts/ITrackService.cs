using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface ITrackService
    {
        Task<List<PlaylistTrackClientModel>> GetTracksByArtistIdAsync(long artistId, string currentUserId);
    }
}
