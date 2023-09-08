namespace seeds.Dal.Dto.FromDb;

/// <summary>
/// We need a DTO here because I see no other way to
/// convert Family.Topics which has Type List of Topic to 
/// Type List of TopicFromDb.
/// </summary>
public class FamilyFromDb
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "family";
    public string CategoryKey { get; set; } = "NoC";
    public int ProbablePreference { get; set; }
    public List<TopicFromDb> Topics { get; set; } = new();
}
