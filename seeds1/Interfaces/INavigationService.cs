namespace seeds1.Interfaces;

public interface INavigationService
{
    public bool RedrawNavigationTarget { get; set; }
    public Task NavigateToAsync(string url, IDictionary<string, object> navParameters);
}
