﻿using Microsoft.Extensions.Logging;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.Factories;
using seeds1.Interfaces;
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

        builder.Services.AddSingleton<IGlobalService, GlobalService>();
        builder.Services.AddSingleton<IGenericFactory<LoginViewModel>, LoginViewModelFactory>();
        builder.Services.AddSingleton<IGenericFactory<FeedEntryViewModel>, FeedEntryViewModelFactory>();
        builder.Services.AddSingleton<IGenericFactory<FeedViewModel>, FeedViewModelFactory>();
        builder.Services.AddSingleton<IGenericFactory<PreferencesViewModel>, PreferencesViewModelFactory>();
        builder.Services.AddSingleton<IFeedEntriesService, FeedEntriesService>();
        builder.Services.AddSingleton<ICatagPreferencesService, CatagPreferencesService>();
        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddTransient<MyBaseViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<FeedViewModel>();
        builder.Services.AddTransient<FeedEntryViewModel>();
        builder.Services.AddTransient<PreferencesViewModel>();
        builder.Services.AddTransient<DetailViewModel>();
        builder.Services.AddTransient<AddViewModel>();

        //It's mandatory to register also the pages where we DI the VMs!
        /* Although the Pages are registered as transient, they might not
         * be re-painted upon navigation. This is NOT because the DI doesn't 
         * function properly. It's because a Page is cached and navigation to 
         * it simply doesn't re-call the constructor.
         */
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<FeedPage>();
        builder.Services.AddTransient<FeedEntryView>();
        builder.Services.AddTransient<PreferencesPage>();
        builder.Services.AddTransient<DetailPage>();
        builder.Services.AddTransient<AddPage>();


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
