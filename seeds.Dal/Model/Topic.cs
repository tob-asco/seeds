using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/// <summary>
/// Entity representing a Topic of an Idea.
/// The tuple (CategoryKey, Name) is constrained to be unique.
/// Ideas should be topicged if they are interesting to the community that is defined by the topic.
/// Synonyms: "Community", "Interessensgemeinschaft".
/// </summary>
[Table("topics")]
public class Topic
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    [Column("name")]
    public string Name { get; set; } = "topic";

    /// <summary>
    /// Null if the Topic is an orphan (i.e. has no Family)
    /// </summary>
    [Column("family_id")]
    public Guid? FamilyId { get; set; }

    #region Navigation
    public Category Category { get; set; } = null!; // the parent category (required)
    public List<Idea> Ideas { get; set; } = new(); // ideas that carry this Topic
    public List<User> Users { get; } = new(); // user interactions with this Topic
    public List<UserPreference> CatopicUserPreferences { get; } = new();
    public Family? Family { get; } // if it has a family, this is it
    #endregion
}
