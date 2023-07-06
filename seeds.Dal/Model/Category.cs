using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Model;

[Table("categories")]
public class Category
{
    [Column("key")]
    [Key]
    public string Key { get; set; } = "NoC"; //GAD, ITE, ENV, H4H, ...

    [Column("name")]
    public string Name { get; set; } = "No Category";

    [Column("tags")]
    public List<string> Tags { get; set; } = new();
}
