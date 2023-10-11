using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface IPlaylistService
    {
        Task<PlaylistClientModel> GetPlayListByIdAsync(long playlistId);

        Task<List<PlaylistClientModel>> GetPlayListsAsync();

        Task<List<PlaylistClientModel>> SearchPlaylistsAsync(string playlistName);

    }
}
