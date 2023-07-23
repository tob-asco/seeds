using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface ICategoryPreferencesService
{
    public Task<List<CatPreference>> GetCatPreferencesAsync();
    public int StepCatPreference(int oldPreference);
}
