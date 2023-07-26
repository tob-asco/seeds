namespace seeds.Dal.Dto.ToAndFromDb;

public class CategoryDto
{
    public string Key { get; set; } = "NoC"; //GAD, ITE, ENV, H4H, ...
    public string Name { get; set; } = "No Category";

    public override string ToString()
    {
        return Key + " - " + Name;
    }
}
