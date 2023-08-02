using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;

namespace seeds.Dal.Services;

public class TagService : ITagService
{
    private readonly IDalBaseService baseService;

    public TagService(IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<TagDto>> GetTagsAsync()
    {
        string url = $"api/Tags";
        var baseResult = await baseService.GetDalModelAsync<List<TagDto>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<TagDto> GetTagAsync(string categoryKey, string name)
    {
        string url = $"api/Tags/{categoryKey}/{name}";
        var baseResult = await baseService.GetDalModelAsync<TagDto>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
