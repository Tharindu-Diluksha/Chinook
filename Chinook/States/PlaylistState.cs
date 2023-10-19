using Chinook.ClientModels;

namespace Chinook.States
{
    public class PlaylistState
    {
        public event Action OnChange;

        private List<Playlist> playlists;

        public List<Playlist> Playlists
        {
            get => playlists;
            set
            {
                playlists = value;
                NotifyStateChanged();
            }
        }

        public void AddPlaylist(Playlist playlist)
        {
            playlists.Add(playlist);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
