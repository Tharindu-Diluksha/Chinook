using Chinook.ClientModels;
using Chinook.Constants;
using Chinook.Contracts;
using Chinook.Models;
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

        public async Task AddTrackToPlaylistAsync(long playlistId, long trackId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var updatingPlaylist = await dbContext.Playlists.FirstOrDefaultAsync(p => p.PlaylistId == playlistId);
            if (updatingPlaylist != null)
            {
                var track = await dbContext.Tracks.FirstOrDefaultAsync(t => t.TrackId == trackId);
                if (track != null)
                    updatingPlaylist.Tracks.Add(track);
                    await dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveTrackFromPlaylistAsync(long playlistId, long trackId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var updatingPlaylist = await dbContext.Playlists.Include(p => p.Tracks).FirstOrDefaultAsync(p => p.PlaylistId == playlistId);
            if (updatingPlaylist != null)
            {
                var track = await dbContext.Tracks.FirstOrDefaultAsync(t => t.TrackId == trackId);
                if (track != null)
                    updatingPlaylist.Tracks.Remove(track);
                    await dbContext.SaveChangesAsync();
            }
        }

        public async Task<long> CreateNewPlaylistAsync(string playlistName, string currentUserId)
        {
            // Validate for duplicate playlist
            if (await IsDuplicatePlaylistForUserAsync(playlistName, currentUserId))
                throw new Exception($"Duplicate Playlist {playlistName}");

            using var dbContext = _dbFactory.CreateDbContext();
            Playlist newPlaylist = new Playlist { Name = playlistName };
            UserPlaylist newUserPlaylist = new UserPlaylist { PlaylistId = newPlaylist.PlaylistId, UserId = currentUserId };
            newPlaylist.UserPlaylists = new List<UserPlaylist> { newUserPlaylist };
            dbContext.Playlists.Add(newPlaylist);
            await dbContext.SaveChangesAsync();
            return newPlaylist.PlaylistId;
        }

        public async Task<long> GetFavouritePlaylistIdOfUserAsync(string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var userPlaylist = await dbContext.UserPlaylists
                .Include(up => up.Playlist)
                .FirstOrDefaultAsync(up => up.UserId == currentUserId && up.Playlist.Name == CommonConstant.UserFavouritePlayListName);

            return userPlaylist?.Playlist.PlaylistId ?? -1;
        }

        public async Task<PlaylistClientModel> GetPlayListByIdAsync(long playlistId, string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();

            var playlist = await dbContext.Playlists
                .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                .Where(p => p.PlaylistId == playlistId)
                .Select(p => new PlaylistClientModel()
                {
                    Name = p.Name,
                    Tracks = p.Tracks.Select(t => new ClientModels.PlaylistTrackClientModel()
                    {
                        AlbumTitle = t.Album != null ? t.Album.Title : string.Empty,
                        ArtistName = t.Album != null ? t.Album.Artist.Name : string.Empty,
                        TrackId = t.TrackId,
                        TrackName = t.Name,
                        IsFavorite = t.Playlists
                            .Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == CommonConstant.UserFavouritePlayListName))
                            .Any()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return playlist;
        }

        public async Task<List<PlaylistClientModel>> GetPlayListsAsync(string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var userPlaylist = await dbContext.UserPlaylists
                .Include(up => up.Playlist)
                .Where(up => up.UserId == currentUserId)
                .Select (up => new PlaylistClientModel { Name =  up.Playlist.Name, Id = up.PlaylistId})
                .ToListAsync();
            return userPlaylist;
        }

        public async Task<List<PlaylistClientModel>> SearchPlaylistsAsync(string playlistName)
        {
            throw new NotImplementedException();
        }


        private async Task<bool> IsDuplicatePlaylistForUserAsync(string playlistName, string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var isDuplicate = await dbContext.UserPlaylists
                .Include(up => up.Playlist)
                .AnyAsync(up => up.UserId == currentUserId && up.Playlist.Name == playlistName);

            return isDuplicate;
        }
    }
}
