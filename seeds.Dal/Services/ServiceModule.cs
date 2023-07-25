using Microsoft.Extensions.DependencyInjection;
using seeds.Dal.Interfaces;
using seeds.Dal.Wrappers;

namespace seeds.Dal.Services;

public static class ServiceModule
{
    public static void DIregistration(IServiceCollection service)
    {
        service.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
        service.AddScoped<IDalBaseService, DalBaseService>();
        service.AddScoped<IIdeasService, IdeasService>();
        service.AddScoped<IUsersService, UsersService>(); //AddScoped suitable for Web Apps
        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<ICategoryUserPreferenceService, CategoryUserPreferenceService>();
        service.AddScoped<IUserIdeaInteractionService, UserIdeaInteractionService>();
        service.AddScoped<IPresentationService, PresentationService>();
    }                               
}
