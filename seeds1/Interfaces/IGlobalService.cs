using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface IGlobalService : IDisposable
{
    public UserDto CurrentUser { get; set; }
    /// <summary>
    /// A convenient list of all FamilyOrPreferences for when to display all tags.
    /// </summary>
    public List<FamilyOrPreference> FamilyOrPreferences { get; set; }

    /// <summary>
    /// Loads UserPreferences of the CurrentUser, to be retrieved by GetPreferences().
    /// </summary>
    public Task LoadPreferencesAsync();
    public Dictionary<Guid, UserPreference> GetPreferences();
    /// <summary>
    /// Change the preference in the globalService member and the DB.
    /// This Method catches any earlier exception.
    /// </summary>
    public Task GlobChangePreferenceAsync(Guid itemId, int newValue);

    /// <summary>
    /// Loads UserIdeaInteractions of the CurrentUser, to be retrieved by GetIdeaInteractions().
    /// </summary>
    public Task LoadIdeaInteractionsAsync();
    public Dictionary<int, UserIdeaInteraction> GetIdeaInteractions();
    /// <summary>
    /// Change the interaction in the globalService member and the DB.
    /// This Method catches any earlier exception.
    /// </summary>
    public Task GlobChangeIdeaInteractionAsync(UserIdeaInteraction newUii);
}
