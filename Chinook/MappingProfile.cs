using AutoMapper;
using Chinook.Constants;
using Chinook.Models;

namespace Chinook
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ClientModels.Artist>()
                .ForMember(dest => dest.AlbumCount, opt => opt.MapFrom(src => src.Albums.Count));

            CreateMap<Track, ClientModels.PlaylistTrack>()
                .ForMember(dest => dest.AlbumTitle, opt => opt.MapFrom(src => src.Album != null ? src.Album.Title : string.Empty))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Album != null && src.Album.Artist != null ? src.Album.Artist.Name : string.Empty))
                .ForMember(dest => dest.TrackId, opt => opt.MapFrom(src => src.TrackId))
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest, context) =>
                {
                    var currentUserId = context.Items["CurrentUserId"].ToString();
                    dest.IsFavorite = src.Playlists.Any(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == CommonConstant.UserFavouritePlayListName));
                });

            CreateMap<Playlist, ClientModels.Playlist>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src => src.Tracks));

            CreateMap<UserPlaylist, ClientModels.Playlist>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Playlist.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PlaylistId));
        }
    }
}