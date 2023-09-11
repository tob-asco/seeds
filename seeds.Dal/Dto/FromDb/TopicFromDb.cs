namespace seeds.Dal.Dto.FromDb;

public class TopicFromDb
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = "topic";

    public string CategoryKey { get; set; } = "NoC";

    public Guid? FamilyId { get; set; }
}
