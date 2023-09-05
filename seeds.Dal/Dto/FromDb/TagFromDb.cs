namespace seeds.Dal.Dto.FromDb;

public class TagFromDb
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "tag";

    public string CategoryKey { get; set; } = "NoC";

    public Guid? FamilyId { get; set; }
}
