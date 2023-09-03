using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserDto CurrentUser { get; set; }

    /// <summary>
    /// Loads UserPreferences of the CurrentUser, to be retrieved by GetPreferences().
    /// </summary>
    public Task LoadPreferencesAsync();
    public Dictionary<Guid, UserPreference> GetPreferences();
    /// <summary>
    /// Change the preference in the globalService member and the DB.
    /// This Method catches any earlier exception.
    /// </summary>
    public Task GlobChangePreference(Guid itemId, int newValue);

    /// <summary>
    /// Loads ButtonedTags of the CurrentUser, to be retrieved by GetButtonedTags().
    /// </summary>
    public Task LoadButtonedTagsAsync();
    public Dictionary<Guid, TagFromDb> GetButtonedTags();

    /// <summary>
    /// Loads UserIdeaInteractions of the CurrentUser, to be retrieved by GetIdeaInteractions().
    /// </summary>
    public Task LoadIdeaInteractionsAsync();
    public Dictionary<int, UserIdeaInteraction> GetIdeaInteractions();
}
