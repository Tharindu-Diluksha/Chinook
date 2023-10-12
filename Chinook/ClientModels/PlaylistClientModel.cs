namespace Chinook.ClientModels;

public class PlaylistClientModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<PlaylistTrackClientModel> Tracks { get; set; }
}