using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface IPlaylistService
    {
        Task<long> CreateNewPlaylistAsync(string playlistName, string currentUserId);

        Task AddTrackToPlaylistAsync(long playlistId, long trackId);

        Task<PlaylistClientModel> GetPlayListByIdAsync(long playlistId, string currentUserId);

        Task<long> GetFavouritePlaylistIdOfUserAsync(string userId);

        Task<List<PlaylistClientModel>> GetPlayListsAsync();

        Task RemoveTrackFromPlaylistAsync(long playlistId, long trackId);

        Task<List<PlaylistClientModel>> SearchPlaylistsAsync(string playlistName);
    }
}
