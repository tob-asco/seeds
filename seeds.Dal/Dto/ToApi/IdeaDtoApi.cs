namespace seeds.Dal.Dto.ToApi;

public class IdeaDtoApi
{
    public int Id { get; set; }
    public string Title { get; set; } = "Idea's Short Title";
    public string Slogan { get; set; } = "Idea's Short Slogan";
    public string CreatorName { get; set; } = "tobi";
    //public DateTime CreationTime { get; set; } = DateTime.Now;
    public string CategoryKey { get; set; } = "NoC";
    public string Slide1 { get; set; } = "https://images.twinkl.co.uk/tr/raw/upload/u/ux/lightbulb-1875247-1920_ver_1.jpg";
    public string Slide2 { get; set; } = "";
    public string Slide3 { get; set; } = "";

}
