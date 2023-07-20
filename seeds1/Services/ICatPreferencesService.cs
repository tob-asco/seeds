using seeds.Dal.Dto.ToApi;
using seeds1.MauiModels;

namespace seeds1.Services;

public interface ICatPreferencesService
{
    public UserDtoApi CurrentUser { get; set; }
    public Task<IEnumerable<CatPreference>> GetCatPreferencesAsync();
}
