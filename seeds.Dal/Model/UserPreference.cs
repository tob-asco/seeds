using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Model;

/* This entity serves as the "join entity" between Category & User.
 * Additionally it comes with the "payload" of the actual preference.
 * Here we opt for "Many-to-Many and join table with payload".
 * cf. https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-and-join-table-with-payload
 */

[Table("user_preference")]
public class UserPreference
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";
    [Column("tag_name")]
    [AllowNull] // if null in DB, then this is a category preference
    public string? TagName { get; set; }
    [Column("username")]
    public string Username { get; set; } = "tobi";

    #region Payload
    [Column("value")]
    public int Value { get; set; } = 0;
    #endregion
}