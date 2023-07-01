using Microsoft.Extensions.Logging;
using seeds.Dal.Services;
using seeds1.Services;

namespace seeds1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        //The following builder addings register possible dependencies to the
        //DI (Dependency Injection) container:
        //(This means that constructors of e.g. ViewModels can implement
        // these dependencies as parameters and we do not have to change
        // the lines where the corresponding VM objects are created because
        // the DI container knows about possible dependencies and just 
        // provides those instances - with standard values.)

        //call the DI registration method of the DAL project
        ServiceModule.DIregistration(builder.Services);

        builder.Services.AddScoped<IUsersService, UsersService>(); //AddScoped suitable for Web Apps
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddTransient<BasisViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<FeedViewModel>();

        //It's mandatory to register also the pages where we DI the VMs!
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<FeedPage>();


        /******************* auto-generated ********************/
        builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
#if DEBUG
        builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
