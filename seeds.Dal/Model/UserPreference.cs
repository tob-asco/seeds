using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Model;

/* This entity serves as the "join entity" between Category & User.
 * Additionally it comes with the "payload" of the actual preference.
 * Here we opt for "Many-to-Many and join table with payload".
 * cf. https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-and-join-table-with-payload
 */

/// <summary>
/// Join Entity that may be used for a tuple of (User, {entity with Guid Id}).
/// It comes with an int Payload "Value".
/// </summary>
[Table("user_preference")]
public class UserPreference
{
    [Column("item_id")]
    public Guid ItemId { get; set; } = new();

    [Column("username")]
    public string Username { get; set; } = "tobi";

    #region Payload
    [Column("value")]
    public int Value { get; set; } = 0;
    #endregion
}