using AutoMapper;
using Chinook.ClientModels;
using Chinook.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IDbContextFactory<ChinookContext> _dbFactory;
        private readonly IMapper _mapper;

        public ArtistService(IDbContextFactory<ChinookContext> dbFactory, IMapper mapper)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<Artist> GetArtistByIdAsync(long artistId)
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var artist = await dbContext.Artists
                .Include(a => a.Albums)
                .SingleOrDefaultAsync(a => a.ArtistId == artistId);

            var clientArtist = _mapper.Map<Artist>(artist);   

            return clientArtist;
        }

        public async Task<List<Artist>> GetArtistsAsync(string artistName = "")
        {
            using var dbContext = _dbFactory.CreateDbContext();
            var artists = await dbContext.Artists
                .Where(a => (string.IsNullOrEmpty(artistName)) || (!string.IsNullOrEmpty(artistName) && a.Name != null && a.Name.ToUpper().Contains(artistName.ToUpper())))
                .Include(a => a.Albums)
                .ToListAsync();

            var clientArtists = _mapper.Map<List<Artist>>(artists);

            return clientArtists;
        }
    }
}
