using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("tags")]
public class Tag
{
    [Column("name")]
    public string Name { get; set; } = "tag";

    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    #region Navigation
    public Category Category { get; set; } = null!;
    public List<User> Users { get; } = new();
    public List<CategoryUserPreference> CategoryUserPreferences { get; } = new();
    #endregion
}
