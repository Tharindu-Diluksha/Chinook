using Chinook.ClientModels;
using Chinook.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;

        public ArtistService(IDbContextFactory<ChinookContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public async Task<ArtistClientModel> GetArtistByIdAsync(long artistId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var artist = await dbContext.Artists
                .Include(a => a.Albums)
                .Select(a => new ArtistClientModel
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name,
                    AlbumCount = a.Albums.Count
                })
                .SingleOrDefaultAsync(a => a.ArtistId == artistId);

            return artist;
        }

        public async Task<List<ArtistClientModel>> GetArtistsAsync(string artistName = "")
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var artists = await dbContext.Artists
                .Where(a => (string.IsNullOrEmpty(artistName)) || (!string.IsNullOrEmpty(artistName) && a.Name != null && a.Name.ToUpper().Contains(artistName.ToUpper())))
                .Include(a => a.Albums)
                .Select(a => new ArtistClientModel
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name,
                    AlbumCount = a.Albums.Count
                })
                .ToListAsync();
            return artists;
        }
    }
}
