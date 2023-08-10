using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("idea_tag")]
public class IdeaTag
{
    [Column("idea_id")]
    public int IdeaId { get; set; }

    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    [Column("tag_name")]
    public string TagName { get; set; } = "tag";

}
