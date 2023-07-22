using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using seeds1.Interfaces;

namespace seeds1.Services;

public class NavigationService : INavigationService
{
    public bool RedrawNavigationTarget { get; set; }

    public async Task NavigateToAsync(string url, IDictionary<string, object> navParameters)
    {
        await Shell.Current.GoToAsync(url, true, navParameters);
    }
}
