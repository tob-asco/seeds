using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("idea_tag")]
public class IdeaTag
{
    [Column("idea_id")]
    public int IdeaId { get; set; }

    [Column("tag_id")]
    public Guid TagId { get; set; }

}
