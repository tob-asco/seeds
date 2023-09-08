using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class StaticService : IStaticService
{
    private readonly ICategoryService categoryService;
    private readonly IFamilyService familyService;
    private readonly ITopicService topicService;

    private Dictionary<string, CategoryDto> Categories { get; set; }
    private Dictionary<Guid, FamilyFromDb> Families { get; set; }
    private Dictionary<Guid, TopicFromDb> Topics { get; set; }
    private bool CatsLoaded { get; set; } = false;
    private bool FamsLoaded { get; set; } = false;
    private bool TopicsLoaded { get; set; } = false;
    public StaticService(
        ICategoryService categoryService,
        IFamilyService familyService,
        ITopicService topicService)
    {
        this.categoryService = categoryService;
        this.familyService = familyService;
        this.topicService = topicService;
    }

    public Dictionary<string, CategoryDto> GetCategories()
    {
        if (!CatsLoaded)
        { throw new InvalidOperationException("Categories not yet loaded."); }
        else { return Categories; }
    }

    public Dictionary<Guid, FamilyFromDb> GetFamilies()
    {
        if (!FamsLoaded)
        { throw new InvalidOperationException("Families not yet loaded."); }
        else { return Families; }
    }

    public Dictionary<Guid, TopicFromDb> GetTopics()
    {
        if (!TopicsLoaded)
        { throw new InvalidOperationException("Topics not yet loaded."); }
        else { return Topics; }
    }

    public async Task LoadCategoriesAsync()
    {
        if (!CatsLoaded)
        {
            // retrieve
            var list = await categoryService.GetCategoriesAsync();

            // convert and inform
            Categories = list.ToDictionary(c => c.Key);
            CatsLoaded = true;
        }
    }
    public async Task LoadFamiliesAsync()
    {
        if (!FamsLoaded)
        {
            // retrieve
            var list = await familyService.GetFamiliesAsync();

            // convert and inform
            Families = list.ToDictionary(f => f.Id);
            FamsLoaded = true;
        }
    }
    public async Task LoadTopicsAsync()
    {
        if (!TopicsLoaded)
        {
            // retrieve
            var list = await topicService.GetTopicsAsync();

            // convert and inform
            Topics = list.ToDictionary(t => t.Id);
            TopicsLoaded = true;
        }
    }

    public async Task LoadStaticsAsync()
    {
        await LoadCategoriesAsync();
        await LoadFamiliesAsync();
        await LoadTopicsAsync();
    }
}
