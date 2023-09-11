using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("idea_topic")]
public class IdeaTopic
{
    [Column("idea_id")]
    public int IdeaId { get; set; }

    [Column("topic_id")]
    public Guid TopicId { get; set; }

}
