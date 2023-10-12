using Chinook.ClientModels;

namespace Chinook.States
{
    public class PlaylistState
    {
        public event Action OnChange;

        private List<PlaylistClientModel> playlists;

        public List<PlaylistClientModel> Playlists
        {
            get => playlists;
            set
            {
                playlists = value;
                NotifyStateChanged();
            }
        }

        public void AddPlaylist(PlaylistClientModel playlist)
        {
            playlists.Add(playlist);
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
