using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface IPlaylistService
    {
        Task<long> CreateNewPlaylistAsync(string playlistName, string currentUserId);

        Task AddTrackToPlaylistAsync(long playlistId, long trackId);

        Task<Playlist> GetPlayListByIdAsync(long playlistId, string currentUserId);

        Task<long> GetFavouritePlaylistIdOfUserAsync(string userId);

        Task<List<Playlist>> GetPlaylistsAsync(string currentUserId);

        Task RemoveTrackFromPlaylistAsync(long playlistId, long trackId);

        Task<List<Playlist>> SearchPlaylistsAsync(string playlistName);
    }
}
