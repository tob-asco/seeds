using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CatagPreferencesService : ICatagPreferencesService
{
    private readonly IStaticService staticService;
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly IUserPreferenceService cupService;
    private readonly ITagService tagService;
    private readonly IIdeaTagService ideaTagService;

    public CatagPreferencesService(
        IStaticService staticService,
        IGlobalService globalService,
        ICategoryService categoryService,
        IUserPreferenceService cupService,
        ITagService tagService,
        IIdeaTagService ideaTagService)
    {
        this.staticService = staticService;
        this.globalService = globalService;
        this.categoryService = categoryService;
        this.cupService = cupService;
        this.tagService = tagService;
        this.ideaTagService = ideaTagService;
    }

    public List<CatagPreference> AssembleButtonedUserPreferences()
    {
        List<CatagPreference> prefs = new();
        foreach(var tagKVP in globalService.GetButtonedTags())
        {
            prefs.Add(new()
            {
                Tag = tagKVP.Value,
                Preference = globalService.GetPreferences().ContainsKey(tagKVP.Key) ?
                    globalService.GetPreferences()[tagKVP.Key].Value : 0,
            });
        }
        return prefs;
    }

    //public async Task<List<CatagPreference>> GetTagPreferencesOfIdeaAsync(IdeaFromDb idea)
    //{
    //    List<CatagPreference> catagPrefs = new();
    //    var tags = await ideaTagService.GetTagsOfIdeaAsync(idea.Id);
    //    if (tags.Count == 0) { return  catagPrefs; }
    //    foreach (var tag in tags)
    //    {
    //        var cup = await cupService.GetCatagUserPreferenceAsync(
    //            tag.CategoryKey, globalService.CurrentUser.Username, tag.Name);
    //        catagPrefs.Add(new()
    //        {
    //            CategoryKey = tag.CategoryKey,
    //            TagName = tag.Name,
    //            Preference = cup.Value,
    //        });
    //    }
    //    return catagPrefs;
    //}

    //public async Task<List<CatagPreference>> GetCatagPreferencesAsync()
    //{
    //    List<CatagPreference> catagPrefs = new();
    //    var cats = await categoryService.GetCategoriesAsync();
    //    if (cats.Count == 0) { throw new Exception("No Categories returned."); }
    //    foreach (var cat in cats)
    //    {
    //        var cup = await cupService.GetCatagUserPreferenceAsync(
    //            cat.Key, globalService.CurrentUser.Username);
    //        catagPrefs.Add(new()
    //        {
    //            CategoryKey = cat.Key,
    //            CategoryName = cat.Name,
    //            Preference = cup.Value
    //        });
    //    }
    //    var tags = await tagService.GetTagsAsync();
    //    if (tags.Count == 0) { throw new Exception("No Tags returned."); }
    //    foreach (var tag in tags)
    //    {
    //        var tagsCat = cats.First(c => c.Key == tag.CategoryKey);
    //        var cup = await cupService.GetCatagUserPreferenceAsync(
    //            tag.CategoryKey, globalService.CurrentUser.Username, tag.Name);
    //        catagPrefs.Add(new()
    //        {
    //            CategoryKey = tag.CategoryKey,
    //            CategoryName = tagsCat.Name,
    //            TagName = tag.Name,
    //            Preference = cup.Value
    //        });
    //    }
    //    return catagPrefs;
    //}
    public int StepPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
