using AutoMapper;
using Chinook.ClientModels;
using Chinook.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService : ITrackService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;
        private readonly IMapper _mapper;

        public TrackService(IDbContextFactory<ChinookContext> dbFactory, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<List<PlaylistTrack>> GetTracksByArtistIdAsync(long artistId, string currentUserId)
        {
            using var dbContext = _dbFactory.CreateDbContext();

            var tracks = await dbContext.Tracks.Where(a => a.Album != null && a.Album.ArtistId == artistId)
                .Include(a => a.Album)
                .Include(t => t.Playlists).ThenInclude(tp => tp.UserPlaylists)
                .ToListAsync();

            var playlistTracks = _mapper.Map<List<PlaylistTrack>>(tracks, opts =>
            {
                opts.Items["CurrentUserId"] = currentUserId;
            });

            return playlistTracks;
        }
    }
}
