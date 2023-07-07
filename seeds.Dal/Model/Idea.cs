using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace seeds.Dal.Model;

[Table("ideas")]
public class Idea
{
    [Column("id")]
    [Key] //PRIMARY KEY
    public int Id { get; set; } //maybe use (mix of) other properties as PK

    [Column("title")]
    public string Title { get; set; } = "Idea's Short Title";

    [Column("slogan")]
    public string Slogan { get; set; } = "Idea's Short Slogan";

    [Column("creator")]
    public string Creator { get; set; } = "Creator's username";

    [Column("creation_time")]
    public DateTime CreationTime { get; set; } = DateTime.Now;

    [Column("category_key")]
    public string CategoryKey { get; set; } = "NoC";

    #region Navigation
    public Category Category { get; } = null!;
    #endregion

    [Column("upvotes")]
    public int Upvotes { get; set; } = 0;

    [Column("slide1")]
    [AllowNull] //freedom? Or should there always be an image?
    public string Slide1 { get; set; } = "https://images.twinkl.co.uk/tr/raw/upload/u/ux/lightbulb-1875247-1920_ver_1.jpg";

    [Column("slide2")]
    [AllowNull] //nullable (only for C#; cf. fluent API)
    public string Slide2 { get; set; } = "";

    [Column("slide3")]
    [AllowNull] //nullable (only for C#; cf. fluent API)
    public string Slide3 { get; set; } = "";

}
