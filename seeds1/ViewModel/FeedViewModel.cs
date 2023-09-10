using CommunityToolkit.Maui.Core.Extensions;
using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : MyBaseViewModel
{
    private static readonly int _feedEntryPageSize = 5;
    private readonly IGlobalService glob;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService prefService;
    ObservableCollection<FeedEntryViewModel> feedentryVMs;
    public ObservableCollection<FeedEntryViewModel> FeedentryVMs => feedentryVMs;
    public FeedViewModel(
        IStaticService stat,
        IGlobalService glob,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IUserPreferenceService prefService)
        : base(stat, glob)
    {
        this.glob = glob;
        this.feedEntryVmFactory = feedEntryVmFactory;
        this.prefService = prefService;

        glob.PropertyChanged += OnGlobPropertyChanged;
        feedentryVMs = glob.FeedentryVMs;
    }

    private void OnGlobPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(glob.FeedentryVMs))
        {
            if (feedentryVMs != glob.FeedentryVMs)
            {
                feedentryVMs = glob.FeedentryVMs;
                OnPropertyChanged(nameof(FeedentryVMs));
            }
        }
    }

    [RelayCommand]
    public async Task ChangeTopicPreference(MauiPreference mauiPref)
    {
        await glob.GlobChangePreferenceAsync(
            mauiPref.Topic.Id, prefService.StepPreference(mauiPref.Preference));
    }

    [RelayCommand]
    public async Task MoreFeedentries()
    {
        await glob.MoreFeedentriesAsync();
    }
}
