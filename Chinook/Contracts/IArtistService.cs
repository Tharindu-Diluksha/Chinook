using Chinook.ClientModels;

namespace Chinook.Contracts
{
    public interface IArtistService
    {
        Task<ArtistClientModel> GetArtistByIdAsync(long artistId);

        Task<List<ArtistClientModel>> GetArtistsAsync();
    }
}
