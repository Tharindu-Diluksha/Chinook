using AutoMapper;
using Chinook.ClientModels;
using Chinook.Constants;
using Chinook.Contracts;
using Chinook.Exceptions;
using Chinook.Models;
using Chinook.States;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;
        private readonly PlaylistState _playlistState;
        private readonly IMapper _mapper;

        public PlaylistService(IDbContextFactory<ChinookContext> dbFactory, PlaylistState playlistState, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _playlistState = playlistState;
            _mapper = mapper;
        }

        public async Task AddTrackToPlaylistAsync(long playlistId, long trackId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var updatingPlaylist = await dbContext.Playlists.Include(p => p.Tracks).FirstOrDefaultAsync(p => p.PlaylistId == playlistId);
            if (updatingPlaylist != null)
            {
                if (updatingPlaylist.Tracks.Any(t => t.TrackId == trackId))
                {
                    throw new DuplicateRecordException("Track already in the playlist");
                }
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
                throw new DuplicateRecordException($"Error duplicate playlist:- {playlistName}");

            using var dbContext = _dbFactory.CreateDbContext();
            Models.Playlist newPlaylist = new Models.Playlist { Name = playlistName };
            UserPlaylist newUserPlaylist = new UserPlaylist { PlaylistId = newPlaylist.PlaylistId, UserId = currentUserId };
            newPlaylist.UserPlaylists = new List<UserPlaylist> { newUserPlaylist };
            dbContext.Playlists.Add(newPlaylist);
            await dbContext.SaveChangesAsync();
            _playlistState.AddPlaylist(new ClientModels.Playlist { Id = newPlaylist.PlaylistId, Name = playlistName });

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

        public async Task<ClientModels.Playlist> GetPlayListByIdAsync(long playlistId, string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();

            var playlist = await dbContext.Playlists
                 .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                 .Include(a => a.Tracks).ThenInclude(t => t.Playlists).ThenInclude(tp => tp.UserPlaylists)
                 .Where(p => p.PlaylistId == playlistId)
                 .FirstOrDefaultAsync();

            var clientPlaylist = _mapper.Map<ClientModels.Playlist>(playlist);

            // Map the IsFavorite property
            foreach (var track in clientPlaylist.Tracks)
            {
                track.IsFavorite = playlist.Tracks
                    .Where(t => t.TrackId == track.TrackId && t.Playlists.Any(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == CommonConstant.UserFavouritePlayListName)))
                    .Any();
            }

            return clientPlaylist;
        }

        public async Task<List<ClientModels.Playlist>> GetPlaylistsAsync(string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var userPlaylists = await dbContext.UserPlaylists
                .Include(up => up.Playlist)
                .Where(up => up.UserId == currentUserId)
                .ToListAsync();

            var clientPlaylists = _mapper.Map<List<ClientModels.Playlist>>(userPlaylists);
            return clientPlaylists;
        }

        public async Task<List<ClientModels.Playlist>> SearchPlaylistsAsync(string playlistName)
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
