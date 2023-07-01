using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.Services;

public interface INavigationService
{
    public Task NavigateToAsync(string url, IDictionary<string,object> navParameters);
}
