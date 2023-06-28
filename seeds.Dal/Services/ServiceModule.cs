using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace seeds.Dal.Services;

public static class ServiceModule
{
    public static void DIregistration(IServiceCollection service)
    {
        service.AddSingleton<HttpClient>();
    }
}
