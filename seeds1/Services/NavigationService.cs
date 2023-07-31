using seeds1.Interfaces;

namespace seeds1.Services;

public class NavigationService : INavigationService
{
    public bool RedrawNavigationTarget { get; set; }
    //for testing purposes only
    public bool NavigationCalled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public async Task NavigateToAsync(string url, IDictionary<string, object> navParameters)
    {
        await Shell.Current.GoToAsync(url, true, navParameters);
    }

    public async Task NavigateToAsync(string url)
    {
        await Shell.Current.GoToAsync(url, true);
    }
}
