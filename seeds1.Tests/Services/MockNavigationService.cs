using seeds1.Interfaces;

namespace seeds1.Tests.Services;

public class MockNavigationService : INavigationService
{
    public bool NavigationCalled { get; set; } = false;
    public bool RedrawNavigationTarget { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Task NavigateToAsync(string url, IDictionary<string, object> navParameters)
    {
        NavigationCalled = true;
        return Task.CompletedTask;
    }
}
