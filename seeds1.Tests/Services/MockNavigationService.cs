using seeds1.Interfaces;

namespace seeds1.Tests.Services;

public class MockNavigationService : INavigationService
{
    public bool NavigationCalled { get; set; } = false;
    public bool RedrawNavigationTarget { get; set; } = false;

    public Task NavigateToAsync(string url, IDictionary<string, object> navParameters)
    {
        NavigationCalled = true;
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(string url)
    {
        NavigationCalled = true;
        return Task.CompletedTask;
    }
}
