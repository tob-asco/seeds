using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Dal.Dto.ForMaui;

/// <summary>
/// DTO model intended to be built in endpoints and pretty much
/// ready to be used by VMs without the use of looping over GETs.
/// It doesn't ship with any preferences however, that is to be 
/// populated by the frontend services.
/// </summary>
public class Feedentry
{
    public IdeaFromDb Idea { get; set; } = new();
    public List<TopicFromDb> Topics { get; set; } = new();
    public int Upvotes { get; set; }
}
