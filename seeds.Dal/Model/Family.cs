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
    [Column("probable_preference")]
    public int ProbablePreference { get; set; }

    #region Navigation
    public List<Tag> Tags { get; set; } = new();
    public Category Category { get; } = null!;
    #endregion

    public Family ShallowCopy()
    {
        return new()
        {
            Id = this.Id,
            Name = this.Name,
            CategoryKey = this.CategoryKey,
            ProbablePreference = this.ProbablePreference,
        };
    }
}
