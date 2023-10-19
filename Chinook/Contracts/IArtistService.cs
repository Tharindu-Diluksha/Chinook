using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface IArtistService
    {
        Task<Artist> GetArtistByIdAsync(long artistId);

        Task<List<Artist>> GetArtistsAsync(string artistName = "");
    }
}
