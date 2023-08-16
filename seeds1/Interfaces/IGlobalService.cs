using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserDto CurrentUser { get; set; }
    public Dictionary<Guid, UserPreference> CurrentUserPreferences { set; }
    public Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { set; }

    /// <summary>
    /// Loads UserPrefernces of the CurrentUser, to be retrieved by GetPreferences().
    /// </summary>
    public Task LoadPreferencesAsync();
    public Dictionary<Guid, UserPreference> GetPreferences();
    /// <summary>
    /// Loads UserIdeaInteractions of the CurrentUser, to be retrieved by GetIdeaInteractions().
    /// </summary>
    public Task LoadIdeaInteractionsAsync();
    public Dictionary<int, UserIdeaInteraction> GetIdeaInteractions();
}
