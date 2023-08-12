using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("tags")]
public class Tag
{
    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    [Column("name")]
    public string Name { get; set; } = "tag";

    #region Navigation
    public Category Category { get; set; } = null!; // the parent category (required)
    public List<Idea> Ideas { get; set; } = new(); // ideas that carry this Tag
    public List<User> Users { get; } = new(); // user interactions with this Tag
    public List<CatagUserPreference> CatagUserPreferences { get; } = new();
    #endregion
}
