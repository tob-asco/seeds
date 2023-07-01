using seeds1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.Tests.Services;

public class MockNavigationService : INavigationService
{
    public bool NavigationCalled { get; private set; } = false;
    public Task NavigateToAsync(string url, IDictionary<string, object> navParameters)
    {
        NavigationCalled = true;
        return Task.CompletedTask;
    }
}
