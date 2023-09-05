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
    private readonly ITagService tagService;

    private Dictionary<string, CategoryDto> Categories { get; set; }
    private Dictionary<Guid, Family> Families { get; set; }
    private Dictionary<Guid, TagFromDb> Tags { get; set; }
    private bool CatsLoaded { get; set; } = false;
    private bool FamsLoaded { get; set; } = false;
    private bool TagsLoaded { get; set; } = false;
    public StaticService(
        ICategoryService categoryService,
        IFamilyService familyService,
        ITagService tagService)
    {
        this.categoryService = categoryService;
        this.familyService = familyService;
        this.tagService = tagService;
    }

    public Dictionary<string, CategoryDto> GetCategories()
    {
        if (!CatsLoaded)
        { throw new InvalidOperationException("Categories not yet loaded."); }
        else { return Categories; }
    }

    public Dictionary<Guid, Family> GetFamilies()
    {
        if (!FamsLoaded)
        { throw new InvalidOperationException("Families not yet loaded."); }
        else { return Families; }
    }

    public Dictionary<Guid, TagFromDb> GetTags()
    {
        if (!TagsLoaded)
        { throw new InvalidOperationException("Tags not yet loaded."); }
        else { return Tags; }
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
    public async Task LoadTagsAsync()
    {
        if (!TagsLoaded)
        {
            // retrieve
            var list = await tagService.GetTagsAsync();

            // convert and inform
            Tags = list.ToDictionary(t => t.Id);
            TagsLoaded = true;
        }
    }

    public async Task LoadStaticsAsync()
    {
        await LoadCategoriesAsync();
        await LoadFamiliesAsync();
        await LoadTagsAsync();
    }
}
