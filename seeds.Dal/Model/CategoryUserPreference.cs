using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Model;

/* This entity serves as the "join entity" between Category & User.
 * Additionally it comes with the "payload" of the actual preference.
 * Here we opt for "Many-to-Many and join table with payload".
 * cf. https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-and-join-table-with-payload
 */

[Table("category_user")]
public class CategoryUserPreference
{
    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";
    [Column("user_id")]
    public string Username { get; set; } = "tobi";
    [Column("value")]
    public int Value { get; set; } = 0;
}