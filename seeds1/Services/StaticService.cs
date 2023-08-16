using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.Interfaces;

namespace seeds1.Services;

public class StaticService : IStaticService
{
    private readonly ICategoryService categoryService;
    private readonly ITagService tagService;

    private Dictionary<string, CategoryDto> Categories { get; set; }
    private bool CatsLoaded { get; set; } = false;
    public Dictionary<Guid, Family> Families { get; set; }
    private bool FamsLoaded { get; set; } = false;
    public Dictionary<Guid, TagFromDb> Tags { get; set; }
    private bool TagsLoaded { get; set; } = false;
    public StaticService(
        ICategoryService categoryService,
        ITagService tagService)
    {
        this.categoryService = categoryService;
        this.tagService = tagService;
    }

    public Dictionary<string, CategoryDto> GetCategories()
    {
        if (!CatsLoaded)
        {
            throw new InvalidOperationException("Categories not yet loaded.");
        }
        else { return Categories; }
    }

    public Dictionary<Guid, Family> GetFamilies()
    {
        if (!FamsLoaded)
        {
            throw new InvalidOperationException("Families not yet loaded.");
        }
        else { return Families; }
    }

    public Dictionary<Guid, TagFromDb> GetTags()
    {
        if (!TagsLoaded)
        {
            throw new InvalidOperationException("Tags not yet loaded.");
        }
        else { return Tags; }
    }

    public async Task LoadCategoriesAsync()
    {
        if (!CatsLoaded)
        {
            var list = await categoryService
                .GetCategoriesAsync();
            Categories = list.ToDictionary(c => c.Key);
            CatsLoaded = true;
        }
    }

    public async Task LoadFamiliesAsync()
    {
        if (!FamsLoaded)
        {
            var list = await tagService.GetTagsAsync();
            Tags = list.ToDictionary(t => t.Id);
            FamsLoaded = true;
        }
    }

    public async Task LoadTagsAsync()
    {
        throw new NotImplementedException();
    }
}
