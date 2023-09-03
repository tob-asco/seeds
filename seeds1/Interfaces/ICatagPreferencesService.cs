using seeds.Dal.Dto.FromDb;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface ICatagPreferencesService
{
    /// <summary>
    /// Uses globalService's buttoned tags and their preferences
    /// to build a List of tags that get a button.
    /// </summary>
    /// <returns>List of Tag Preferences</returns>
    public List<CatagPreference> AssembleButtonedUserPreferences();
    ///// <summary>
    ///// Prepares CUP data from CurrentUser of the tags of a specified idea.
    ///// </summary>
    ///// <returns>A short list of CatagPreferences</returns>
    //public Task<List<CatagPreference>> GetTagPreferencesOfIdeaAsync(IdeaFromDb idea);
    ///// <summary>
    ///// Prepares CUP data from CurrentUser of categories and tags
    ///// for usage in the VMs.
    ///// </summary>
    ///// <returns>An unordered list of all CatagPreferences</returns>
    //public Task<List<CatagPreference>> GetCatagPreferencesAsync();
    public int StepPreference(int oldPreference);
}
