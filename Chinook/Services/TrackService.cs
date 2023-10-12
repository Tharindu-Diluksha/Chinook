using Chinook.ClientModels;
using Chinook.Constants;
using Chinook.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService : ITrackService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public TrackService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<List<PlaylistTrackClientModel>> GetTracksByArtistIdAsync(long artistId, string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();

            var tracks = await dbContext.Tracks.Where(a => a.Album != null && a.Album.ArtistId == artistId)
             .Include(a => a.Album)
             .Select(t => new PlaylistTrackClientModel()
             {
                 AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                 TrackId = t.TrackId,
                 TrackName = t.Name,
                 IsFavorite = t.Playlists
                    .Where(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == CommonConstant.UserFavouritePlayListName))
                    .Any()
             })
             .ToListAsync();
            return tracks;
        }
    }
}
