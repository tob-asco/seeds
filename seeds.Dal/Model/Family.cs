using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/// <summary>
/// Entity that enables Tags to be grouped in.
/// </summary>
[Table("families")]
public class Family
{
    [Column("id")]
    [Key]
    public Guid Id { get; set; }
    [Column("name")]
    public string Name { get; set; } = "family";
    [Column("category_key")]
    public string CategoryKey { get; set; } = "Noc";

    #region Navigation
    public List<Tag> Tags { get; } = new();
    public Category Category { get; } = null!;
    #endregion
}
