using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CatopicPreferencesService : ICatopicPreferencesService
{
    private readonly IStaticService staticService;
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly IUserPreferenceService cupService;
    private readonly ITopicService topicService;
    private readonly IIdeaTopicService ideaTopicService;

    public CatopicPreferencesService(
        IStaticService staticService,
        IGlobalService globalService,
        ICategoryService categoryService,
        IUserPreferenceService cupService,
        ITopicService topicService,
        IIdeaTopicService ideaTopicService)
    {
        this.staticService = staticService;
        this.globalService = globalService;
        this.categoryService = categoryService;
        this.cupService = cupService;
        this.topicService = topicService;
        this.ideaTopicService = ideaTopicService;
    }

    //public async Task<List<CatopicPreference>> GetTopicPreferencesOfIdeaAsync(IdeaFromDb idea)
    //{
    //    List<CatopicPreference> catopicPrefs = new();
    //    var topics = await ideaTopicService.GetTopicsOfIdeaAsync(idea.Id);
    //    if (topics.Count == 0) { return  catopicPrefs; }
    //    foreach (var topic in topics)
    //    {
    //        var cup = await cupService.GetCatopicUserPreferenceAsync(
    //            topic.CategoryKey, globalService.CurrentUser.Username, topic.Name);
    //        catopicPrefs.Add(new()
    //        {
    //            CategoryKey = topic.CategoryKey,
    //            TopicName = topic.Name,
    //            Preference = cup.Value,
    //        });
    //    }
    //    return catopicPrefs;
    //}

    //public async Task<List<CatopicPreference>> GetCatopicPreferencesAsync()
    //{
    //    List<CatopicPreference> catopicPrefs = new();
    //    var cats = await categoryService.GetCategoriesAsync();
    //    if (cats.Count == 0) { throw new Exception("No Categories returned."); }
    //    foreach (var cat in cats)
    //    {
    //        var cup = await cupService.GetCatopicUserPreferenceAsync(
    //            cat.Key, globalService.CurrentUser.Username);
    //        catopicPrefs.Add(new()
    //        {
    //            CategoryKey = cat.Key,
    //            CategoryName = cat.Name,
    //            Preference = cup.Value
    //        });
    //    }
    //    var topics = await topicService.GetTopicsAsync();
    //    if (topics.Count == 0) { throw new Exception("No Topics returned."); }
    //    foreach (var topic in topics)
    //    {
    //        var topicsCat = cats.First(c => c.Key == topic.CategoryKey);
    //        var cup = await cupService.GetCatopicUserPreferenceAsync(
    //            topic.CategoryKey, globalService.CurrentUser.Username, topic.Name);
    //        catopicPrefs.Add(new()
    //        {
    //            CategoryKey = topic.CategoryKey,
    //            CategoryName = topicsCat.Name,
    //            TopicName = topic.Name,
    //            Preference = cup.Value
    //        });
    //    }
    //    return catopicPrefs;
    //}
    public int StepPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
