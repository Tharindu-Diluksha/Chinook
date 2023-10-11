namespace Chinook.ClientModels;

public class PlaylistClientModel
{
    public string Name { get; set; }
    public List<PlaylistTrackClientModel> Tracks { get; set; }
}