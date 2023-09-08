using seeds.Dal.Dto.FromDb;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface ICatopicPreferencesService
{
    ///// <summary>
    ///// Prepares CUP data from CurrentUser of the topics of a specified idea.
    ///// </summary>
    ///// <returns>A short list of CatopicPreferences</returns>
    //public Task<List<CatopicPreference>> GetTopicPreferencesOfIdeaAsync(IdeaFromDb idea);
    ///// <summary>
    ///// Prepares CUP data from CurrentUser of categories and topics
    ///// for usage in the VMs.
    ///// </summary>
    ///// <returns>An unordered list of all CatopicPreferences</returns>
    //public Task<List<CatopicPreference>> GetCatopicPreferencesAsync();
    public int StepPreference(int oldPreference);
}
