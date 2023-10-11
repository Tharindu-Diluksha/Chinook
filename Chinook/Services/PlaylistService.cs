using Chinook.ClientModels;
using Chinook.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public PlaylistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<PlaylistClientModel> GetPlayListByIdAsync(long playlistId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlaylistClientModel>> GetPlayListsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<PlaylistClientModel>> SearchPlaylistsAsync(string playlistName)
        {
            throw new NotImplementedException();
        }
    }
}
