﻿using CommunityToolkit.Maui.Core.Extensions;
using MvvmHelpers;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seeds1.Services;

public partial class GlobalService : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IGlobalService
{
    private static readonly int feedEntryPageSize = 5;
    private readonly IStaticService stat;
    private readonly IIdeasService ideasService;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;

    UserDto currentUser;
    public UserDto CurrentUser
    {
        get => currentUser;
        set
        {
            if (value != currentUser)
            {
                currentUser = value;
                if (value != null) // don't send upon Logout
                {
                    OnPropertyChanged(nameof(CurrentUser));
                }
            }
        }
    }

    #region Basic but inaccessible properties used for private consistency
    Dictionary<Guid, UserPreference> CurrentUserPreferences { get; set; } = new();
    Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { get; set; } = new();
    public bool PreferencesLoaded { get; set; } = false; // public, but not in IGlobalService
    private bool IdeaInteractionsLoaded { get; set; } = false;
    #endregion

    #region High-level but inaccessible properties
    Dictionary<string, ObservableCollection<FamilyOrPreference>> fopListDict = new();
    public Dictionary<string, ObservableCollection<FamilyOrPreference>> FopListDict
    {
        get => fopListDict;
        set
        {
            if (value != fopListDict)
            {
                fopListDict = value;
                OnPropertyChanged(nameof(FopListList));
            }
        }
    }
    #endregion

    #region Public properties, converted from the above
    public List<ObservableCollection<FamilyOrPreference>> FopListList => FopListDict.Values.ToList();
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryViewModel> feedentryVMs = new();
    #endregion

    public GlobalService(
        IStaticService stat,
        IIdeasService ideasService,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IUserPreferenceService userPrefService,
        IUserIdeaInteractionService uiiService)
    {
        this.stat = stat;
        this.ideasService = ideasService;
        this.feedEntryVmFactory = feedEntryVmFactory;
        this.userPrefService = userPrefService;
        this.uiiService = uiiService;
    }

    #region OPC stuff
    //public event PropertyChangedEventHandler PropertyChanged;
    //protected virtual void OnPropertyChanged(string propertyName)
    //{
    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //}
    #endregion

    public async Task LoadPreferencesAsync()
    {
        if (!PreferencesLoaded)
        {
            // retrieve
            var userPreferencesList = await userPrefService
                .GetPreferencesOfUserAsync(CurrentUser.Username);
            var userButtonedTopicsList = await userPrefService
                .GetButtonedTopicsOfUserAsync(CurrentUser.Username);

            // convert
            CurrentUserPreferences = userPreferencesList.ToDictionary(up => up.ItemId);

            #region populate FOPs
            // first the families
            List<FamilyOrPreference> fopList = stat.GetFamilies().Values.Select(f =>
                new FamilyOrPreference()
                {
                    CategoryKey = f.CategoryKey,
                    CategoryName = stat.GetCategories()[f.CategoryKey].Name,
                    IsFamily = true,
                    Family = f,
                }).ToList();
            // second all buttoned topics
            fopList.AddRange(
                userButtonedTopicsList.Select(t => new FamilyOrPreference()
                {
                    CategoryKey = t.CategoryKey,
                    CategoryName = stat.GetCategories()[t.CategoryKey].Name,
                    IsFamily = false,
                    Preference = new()
                    {
                        Topic = t,
                        Preference = CurrentUserPreferences.ContainsKey(t.Id) ?
                            CurrentUserPreferences[t.Id].Value : 0,
                    },
                }));
            // now store this list in a grouped dictionary
            FopListDict = fopList
                .GroupBy(fop => fop.CategoryKey)
                .ToDictionary(
                    group => group.Key,
                    group => new ObservableCollection<FamilyOrPreference>(group));
            #endregion

            // inform
            PreferencesLoaded = true;
        }
    }
    public Dictionary<Guid, UserPreference> GetPreferences()
    {
        if (!PreferencesLoaded)
        {
            throw new InvalidOperationException("Preferences not yet loaded.");
        }
        else { return CurrentUserPreferences; }
    }
    public async Task<bool> GlobChangePreferenceAsync(
        Guid itemId, int newValue, bool raisePceFops = true, bool raisePceFeedentries = true)
    {
        bool topicAlreadyButtoned = false;
        try
        {
            // update DB
            await userPrefService.UpsertUserPreferenceAsync(CurrentUser.Username, itemId, newValue);

            #region update FOPs
            if (stat.GetTopics().ContainsKey(itemId) &&
                FopListDict.ContainsKey(stat.GetTopics()[itemId].CategoryKey))
            {
                if (stat.GetTopics()[itemId].FamilyId != null && // Topic is in some family
                    FopListDict[stat.GetTopics()[itemId].CategoryKey].FirstOrDefault(fop =>
                        fop.IsFamily == false &&
                        fop.Preference.Topic.Id == itemId) == null) // .. and not yet in FOPs
                {
                    topicAlreadyButtoned = false;
                    FamilyOrPreference newFop = new()
                    {
                        CategoryKey = stat.GetTopics()[itemId].CategoryKey,
                        IsFamily = false,
                        Preference = new MauiPreference()
                        { Topic = stat.GetTopics()[itemId], Preference = newValue },
                    };
                    FopListDict[stat.GetTopics()[itemId].CategoryKey].Add(newFop);
                }
                else // Topic's already buttoned
                {
                    topicAlreadyButtoned = true;
                    FopListDict[stat.GetTopics()[itemId].CategoryKey].First(fop =>
                        !fop.IsFamily && fop.Preference.Topic.Id == itemId)
                        .Preference.Preference = newValue;
                }
            }
            //if (raisePceFops) { OnPropertyChanged(nameof(FopListList)); }
            #endregion

            #region update member
            if (CurrentUserPreferences.ContainsKey(itemId))
            { CurrentUserPreferences[itemId].Value = newValue; }
            else
            {
                CurrentUserPreferences.Add(itemId, new()
                {
                    Username = CurrentUser.Username,
                    ItemId = itemId,
                    Value = newValue,
                });
            }
            #endregion

            #region update Feedentries
            /* Update all feed entries that have the same topic
             * as the topic clicked.
             * Then update the DB with the new preference.
             */
            for (int i = 0; i < FeedentryVMs.Count; i++)
            {
                // loop over topics
                for (int j = 0; j < FeedentryVMs[i].FeedEntry.MauiPreferences.Count; j++)
                {
                    if (FeedentryVMs[i].FeedEntry.MauiPreferences[j].Topic.Id == itemId)
                    {
                        FeedentryVMs[i].FeedEntry.MauiPreferences[j].Preference = newValue;
                    }
                }
            }
            if (raisePceFeedentries) { OnPropertyChanged(nameof(FeedentryVMs)); }
            #endregion
            return topicAlreadyButtoned;
        }
        catch (Exception ex)
        { await Shell.Current.DisplayAlert("Error", ex.Message, "Ok"); return false; }
    }

    public async Task LoadIdeaInteractionsAsync()
    {
        if (!IdeaInteractionsLoaded)
        {
            var ideaInteractionsList = await uiiService
                .GetIdeaInteractionsOfUserAsync(CurrentUser.Username);
            CurrentUserIdeaInteractions = ideaInteractionsList
                .ToDictionary(uii => uii.IdeaId);
            IdeaInteractionsLoaded = true;
        }
    }
    public Dictionary<int, UserIdeaInteraction> GetIdeaInteractions()
    {
        if (!PreferencesLoaded)
        {
            throw new InvalidOperationException("IdeaInteractions not yet loaded.");
        }
        else { return CurrentUserIdeaInteractions; }
    }
    public async Task GlobChangeIdeaInteractionAsync(UserIdeaInteraction newUii)
    {
        try
        {
            await uiiService.PostOrPutUserIdeaInteractionAsync(newUii);
            if (CurrentUserIdeaInteractions.ContainsKey(newUii.IdeaId))
            {
                CurrentUserIdeaInteractions[newUii.IdeaId] = newUii;
            }
            else { CurrentUserIdeaInteractions.Add(newUii.IdeaId, newUii); }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Upsert Error", ex.Message, "Ok");
        }
    }

    public async Task MoreFeedentriesAsync(
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        int currentPages = (int)Math.Ceiling((decimal)FeedentryVMs.Count / feedEntryPageSize);

        List<UserFeedentry> ufePage = new();
        try
        {
            var fePage = await ideasService.GetFeedentriesPaginatedAsync(
                currentPages + 1, feedEntryPageSize, orderByColumn, isDescending);
            if (fePage.Count == 0) { return; }
            ufePage = fePage.Select(fe => new UserFeedentry()
            {
                Idea = fe.Idea,
                MauiPreferences = fe.Topics.Select(t => new MauiPreference()
                {
                    Topic = t,
                    Preference = CurrentUserPreferences.ContainsKey(t.Id) ?
                            CurrentUserPreferences[t.Id].Value : 0
                }).ToList(),
                Upvoted = CurrentUserIdeaInteractions.ContainsKey(fe.Idea.Id) ?
                        CurrentUserIdeaInteractions[fe.Idea.Id].Upvoted : false,
                Downvoted = CurrentUserIdeaInteractions.ContainsKey(fe.Idea.Id) ?
                        CurrentUserIdeaInteractions[fe.Idea.Id].Upvoted : false,
                Upvotes = fe.Upvotes,
            }).ToList();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error On Collecting FeedEntries", ex.Message, "Ok");
            return;
        }
#if WINDOWS
        if (FeedentryVMs.Count > 0)
        {
            ufePage.Reverse();
        }
#endif
        FeedentryVMs.AddRange(ufePage.Select(ufe =>
        {
            var vm = feedEntryVmFactory.Create();
            vm.FeedEntry = ufe;
            return vm;
        }).ToList());
    }
    public void Dispose()
    {
        CurrentUser = null!;

        CurrentUserPreferences.Clear();
        PreferencesLoaded = false;

        CurrentUserIdeaInteractions.Clear();
        IdeaInteractionsLoaded = false;

        FopListDict.Clear();
        FeedentryVMs.Clear();
    }
}
