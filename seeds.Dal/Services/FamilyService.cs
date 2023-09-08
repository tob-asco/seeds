using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;

namespace seeds.Dal.Services;

public class FamilyService : IFamilyService
{
    private readonly IDalBaseService baseService;
    private readonly string baseUri;

    public FamilyService(
        IDalBaseService baseService)
    {
        baseUri = "api/Families/";
        this.baseService = baseService;
    }
    public async Task<List<FamilyFromDb>> GetFamiliesAsync()
    {
        string url = baseUri;
        return await baseService.GetDalModelAsync<List<FamilyFromDb>?>(url)
            ?? throw baseService.ThrowGetNullException(url);
    }
}
