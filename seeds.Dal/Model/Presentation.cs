using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/* No navigation properties, so also no DTO for this one.
 */

[Table("presentation")]
public class Presentation
{
    [Column("id")]
    [Key] //Primary Key
    public int Id { get; set; }

    [Column("idea_id")]
    public int IdeaId { get; set; }

    [Column("description")]
    public string Description { get; set; } = "";
}
