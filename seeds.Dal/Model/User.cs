using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Model;

[Table("users")]
public class User
{
    //[Column("id")]
    //[Key] //PRIMARY KEY, NOT NULL
    //public int Id { get; set; }

    [Key]
    [Column("username")] //NOT NULL
    public string Username { get; set; } = String.Empty;

    [Column("password")]
    [AllowNull] //nullable (only for C#; cf. fluent API)
    public string Password { get; set; }

    [Column ("email")]
    [AllowNull] //nullable (only for C#; cf. fluent API)
    public string Email { get; set; }

    #region Navigation
    public List<CategoryUserPreference> CategoryUserPreferences { get; } = new();
    public List<Category> Categories { get; } = new();
    public List<Idea> CreatedIdeas { get; set; } = new();
    public List<Idea> InteractedIdeas { get; set; } = new();
    #endregion
}
