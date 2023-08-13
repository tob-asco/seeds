using seeds.Dal.Dto.FromDb;

namespace seeds1.MauiModels;

public class TagFamily
{
    public string Name { get; set; }
    public List<TagFromDb> Tags { get; set; }
}
