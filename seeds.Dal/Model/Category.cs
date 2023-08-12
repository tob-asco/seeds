using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

[Table("categories")]
public class Category
{
    [Column("key")]
    [Key]
    public string Key { get; set; } = "NoC"; //GAD, ITE, ENV, H4H, ...

    [Column("name")]
    public string Name { get; set; } = "No Category";

    #region Navigation
    public List<CatagUserPreference> CatagUserPreferences { get; } = new();
    public List<User> Users { get; } = new();
    public List<Tag> Tags { get; } = new();
    #endregion
}
