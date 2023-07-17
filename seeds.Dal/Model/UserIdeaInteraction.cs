using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/* Join Entity.
 * Relation: User : Idea = N : M
 * Payload: Upvoted, MarkedForLater, ...
 */
[Table("user_idea")]
public class UserIdeaInteraction
{
    [Column("username")]
    public string Username { get; set; } = "tobi";
    [Column("idea_id")]
    public int IdeaId { get; set; } = 0;
    
    #region Payload
    [Column("upvoted")]
    public bool Upvoted { get; set; } = false;
    [Column("downvoted")]
    public bool Downvoted { get; set; } = false;

    #endregion
}
