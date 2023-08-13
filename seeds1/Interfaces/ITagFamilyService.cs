using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface ITagFamilyService
{
    public List<FamilyOrPreference> ConvertToFamilyOrPreferences(List<CatagPreference> catagPrefs);
    public Task<List<FamilyOrPreference>> GetAllFamilyOrPreferenceAsync();
}
