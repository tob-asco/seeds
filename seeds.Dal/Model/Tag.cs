﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace seeds.Dal.Model;

/// <summary>
/// Entity representing a Tag of an Idea.
/// The tuple (CategoryKey, Name) is constrained to be unique.
/// Ideas should be tagged if they are interesting to the community that is defined by the tag.
/// Synonyms: "Community", "Interessensgemeinschaft".
/// </summary>
[Table("tags")]
public class Tag
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    [Column("name")]
    public string Name { get; set; } = "tag";

    /// <summary>
    /// Null if the Tag is an orphan (i.e. has no Family)
    /// </summary>
    [Column("family_id")]
    public Guid? FamilyId { get; set; }

    #region Navigation
    public Category Category { get; set; } = null!; // the parent category (required)
    public List<Idea> Ideas { get; set; } = new(); // ideas that carry this Tag
    public List<User> Users { get; } = new(); // user interactions with this Tag
    public List<CatagUserPreference> CatagUserPreferences { get; } = new();
    public Family? Family { get; } // if it has a family, this is it
    #endregion
}
