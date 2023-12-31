﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/// <summary>
/// Entity to group Topics into quite general classes.
/// Ideas are not assigned a Category, only Topics.
/// </summary>
[Table("categories")]
public class Category
{
    [Column("key")]
    [Key]
    public string Key { get; set; } = "NoC"; //GAD, ITE, ENV, H4H, ...

    [Column("name")]
    public string Name { get; set; } = "No Category";

    #region Navigation
    public List<User> Users { get; } = new();
    public List<Topic> Topics { get; } = new();
    public List<Family> Families { get; } = new();
    #endregion
}
