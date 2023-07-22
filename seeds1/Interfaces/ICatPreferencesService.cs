using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface ICatPreferencesService
{
    public Task<IEnumerable<CatPreference>> GetCatPreferencesAsync();
    public int StepCatPreference(int oldPreference);
}
