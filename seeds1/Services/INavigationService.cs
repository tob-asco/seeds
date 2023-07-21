namespace seeds1.Services;

public interface INavigationService
{
    public bool RedrawNavigationTarget { get; set; }
    public Task NavigateToAsync(string url, IDictionary<string,object> navParameters);
}
