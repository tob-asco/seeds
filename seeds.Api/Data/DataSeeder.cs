using seeds.Dal.Model;

namespace seeds.Api.Data;

public class DataSeeder
{
    private readonly seedsApiContext _dbContext;
    public List<Category> Cats = new();
    public List<Tag> Tags = new();
    public List<User> Users = new();
    public List<CatagUserPreference> Cups = new();
    public List<Idea> Ideas = new();

    public DataSeeder(seedsApiContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        PopulateCats();
        if (!_dbContext.Category.Any()) { _dbContext.Category.AddRange(Cats); }
        else { Cats = _dbContext.Category.ToList(); }

        PopulateTags();
        if (!_dbContext.Tag.Any()) { _dbContext.Tag.AddRange(Tags); }
        else { Tags = _dbContext.Tag.ToList(); }

        PopulateUsers();
        if (!_dbContext.User.Any()) { _dbContext.User.AddRange(Users); }
        else { Users = _dbContext.User.ToList(); }

        PopulateCups();
        if (!_dbContext.CatagUserPreference.Any()) { _dbContext.CatagUserPreference.AddRange(Cups); }
        else { Cups = _dbContext.CatagUserPreference.ToList(); }

        PopulateIdeas();
        if (!_dbContext.Idea.Any()) { _dbContext.Idea.AddRange(Ideas); }
        else { Ideas = _dbContext.Idea.ToList(); }

        _dbContext.SaveChanges();
    }

    public void PopulateCats()
    {
        Cats.Add(new Category { Key = "NoC", Name = "No Category" });
        Cats.Add(new Category { Key = "FEA", Name = "Features" });
        Cats.Add(new Category { Key = "TRA", Name = "Transportation" });
        Cats.Add(new Category { Key = "POW", Name = "Energy" });
        Cats.Add(new Category { Key = "LIFE", Name = "Lifestyle" });
        Cats.Add(new Category { Key = "HOME", Name = "Living" });
        Cats.Add(new Category { Key = "ENV", Name = "Environment" });
        Cats.Add(new Category { Key = "CPU", Name = "Software" });
        Cats.Add(new Category { Key = "H4H", Name = "Human for Human" });
        Cats.Add(new Category { Key = "EDU", Name = "Education" });
        Cats.Add(new Category { Key = "FOOD", Name = "Food & Drinks" });
        Cats.Add(new Category { Key = "HEAL", Name = "Health" });
        Cats.Add(new Category { Key = "NEER", Name = "Engineering" });
        Cats.Add(new Category { Key = "GOV", Name = "Governmental" });
        Cats.Add(new Category { Key = "DSGN", Name = "Design" });
        Cats.Add(new Category { Key = "OUT", Name = "Outdoors" });
        Cats.Add(new Category { Key = "ART", Name = "Arts" });
        Cats.Add(new Category { Key = "MUS", Name = "Music" });
        Cats.Add(new Category { Key = "LIT", Name = "Literature" });
        Cats.Add(new Category { Key = "SOC", Name = "Society" });
        Cats.Add(new Category { Key = "GAD", Name = "Gadgets" });
        Cats.Add(new Category { Key = "SPO", Name = "Sport" });

        Cats.Add(new Category { Key = "FUS", Name = "Fusion" });
        Cats.Add(new Category { Key = "SCI", Name = "Science" });
        Cats.Add(new Category { Key = "???", Name = "Idea Needed" });
        Cats.Add(new Category { Key = "SEEDS", Name = "Internal" });
    }
    public void PopulateTags()
    {
        Tags.Add(new Tag { CategoryKey = "NoC", Name = "fusion" });

        Tags.Add(new Tag { CategoryKey = "FEA", Name = "music app: {main music streaming apps}" });
        Tags.Add(new Tag { CategoryKey = "FEA", Name = "streaming provider: {main video streaming providers}" });
        Tags.Add(new Tag { CategoryKey = "FEA", Name = "social media: {main social platforms}" });
        Tags.Add(new Tag { CategoryKey = "FEA", Name = "online meeting: {main meeting providers}" });
        Tags.Add(new Tag { CategoryKey = "FEA", Name = "operating systems: {Win, Linux, OsX, Android, iOs, and smaller mobile Os}" });
        Tags.Add(new Tag { CategoryKey = "FEA", Name = "games" });

        Tags.Add(new Tag { CategoryKey = "TRA", Name = "electro" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "alternative fuels" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "cars" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "motorbikes" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "bicycle" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "new transportation" });
        Tags.Add(new Tag { CategoryKey = "TRA", Name = "public transport" });

        Tags.Add(new Tag { CategoryKey = "POW", Name = "alternatives" });
        Tags.Add(new Tag { CategoryKey = "POW", Name = "storage" });
        Tags.Add(new Tag { CategoryKey = "POW", Name = "reduction" });

        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "gaming" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "traveling" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "beauty" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "dating" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "too much time" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "life hacks" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "series & movies" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "home plants" });
        Tags.Add(new Tag { CategoryKey = "LIFE", Name = "tinkering" }); //(= basteln)

        Tags.Add(new Tag { CategoryKey = "HOME", Name = "living solutions" });
        Tags.Add(new Tag { CategoryKey = "HOME", Name = "garden" });
        Tags.Add(new Tag { CategoryKey = "HOME", Name = "keep it clean" });

        Tags.Add(new Tag { CategoryKey = "ENV", Name = "CO2 reduction" });
        Tags.Add(new Tag { CategoryKey = "ENV", Name = "climate change" });
        Tags.Add(new Tag { CategoryKey = "ENV", Name = "extreme weather" });
        Tags.Add(new Tag { CategoryKey = "ENV", Name = "save the bees" });
        Tags.Add(new Tag { CategoryKey = "ENV", Name = "reduce plastic" });

        Tags.Add(new Tag { CategoryKey = "CPU", Name = "games" });
        Tags.Add(new Tag { CategoryKey = "CPU", Name = "mobile apps" });
        Tags.Add(new Tag { CategoryKey = "CPU", Name = "databases" });
        Tags.Add(new Tag { CategoryKey = "CPU", Name = "server" });
        Tags.Add(new Tag { CategoryKey = "CPU", Name = "new languages" });
        Tags.Add(new Tag { CategoryKey = "CPU", Name = "hacking / security" });

        Tags.Add(new Tag { CategoryKey = "H4H", Name = "diminishing inequality" });
        Tags.Add(new Tag { CategoryKey = "H4H", Name = "food/water supply" });
        Tags.Add(new Tag { CategoryKey = "H4H", Name = "power supply" });
        Tags.Add(new Tag { CategoryKey = "H4H", Name = "contra unemployment" });

        Tags.Add(new Tag { CategoryKey = "EDU", Name = "teaching style" });
        Tags.Add(new Tag { CategoryKey = "EDU", Name = "educational system" });
        Tags.Add(new Tag { CategoryKey = "EDU", Name = "early development" });

        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "coffee" });
        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "tea" });
        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "cocktails" });
        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "secret recipes" });
        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "hospitality industry" }); //(= Gastronomie)
        Tags.Add(new Tag { CategoryKey = "FOOD", Name = "substitutes" });

        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "healing movements" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "nutrition" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "meditational" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "mental health" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "minimal workout" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "prophylactic lifestyle" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "contra addiction" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "grandma's medicine" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "exercises vs. pain" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "energize me" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "get happy!" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "posture" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "hygiene" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "allergies" });
        Tags.Add(new Tag { CategoryKey = "HEAL", Name = "body region: {major regions like eyes, head, shoulder, back, stomach, hands, ..}" });

        Tags.Add(new Tag { CategoryKey = "GOV", Name = "new law" });
        Tags.Add(new Tag { CategoryKey = "GOV", Name = "adapting law" });
        Tags.Add(new Tag { CategoryKey = "GOV", Name = "reducing law" });
        Tags.Add(new Tag { CategoryKey = "GOV", Name = "for country: {all countries}" }); //(195 countries on Earth)
        Tags.Add(new Tag { CategoryKey = "GOV", Name = "for city: {all cities}" }); //(approx. 10 000 cities on Earth)

        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "furniture" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "architectural" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "stationery" }); //(= Schreibwaren)
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "electronic devices" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "haut couture" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "jewelry" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "autarkic solutions" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "shoes" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "survival" });
        Tags.Add(new Tag { CategoryKey = "DSGN", Name = "watches" });

        Tags.Add(new Tag { CategoryKey = "OUT", Name = "surviving" });
        Tags.Add(new Tag { CategoryKey = "OUT", Name = "survival gear" });
        Tags.Add(new Tag { CategoryKey = "OUT", Name = "camping" });
        Tags.Add(new Tag { CategoryKey = "OUT", Name = "backpacking" });
        Tags.Add(new Tag { CategoryKey = "OUT", Name = "tents" });
        Tags.Add(new Tag { CategoryKey = "OUT", Name = "functional wear" });

        Tags.Add(new Tag { CategoryKey = "ART", Name = "painting" });
        Tags.Add(new Tag { CategoryKey = "ART", Name = "sculpting" });
        Tags.Add(new Tag { CategoryKey = "ART", Name = "movement: {major movements like Modernism, Realism, Cubism, Avant-Garde, ...}" });

        Tags.Add(new Tag { CategoryKey = "MUS", Name = "mood: {electro, party, dancing, heroic, classic, deep, tense, ...}" });
        Tags.Add(new Tag { CategoryKey = "MUS", Name = "local: {all countries}" });

        Tags.Add(new Tag { CategoryKey = "LIT", Name = "poetry: {all languages, maybe also dialects}" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "short: {all languages, maybe also dialects}" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "novel topics" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "sciFi topics" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "sayings" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "one-liners" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "showerthoughts" });
        Tags.Add(new Tag { CategoryKey = "LIT", Name = "for language: {all languages and dialects}" });

        Tags.Add(new Tag { CategoryKey = "SOC", Name = "grand movement" });
        Tags.Add(new Tag { CategoryKey = "SOC", Name = "global crisis" });
        Tags.Add(new Tag { CategoryKey = "SOC", Name = "communities" });

        Tags.Add(new Tag { CategoryKey = "GAD", Name = "w/o electro" });
        Tags.Add(new Tag { CategoryKey = "GAD", Name = "diy" });
        Tags.Add(new Tag { CategoryKey = "GAD", Name = "3d prints" });

        Tags.Add(new Tag { CategoryKey = "SPO", Name = "sports gear: {major sports}" });
        Tags.Add(new Tag { CategoryKey = "SPO", Name = "new moves: {major sports}" });
        Tags.Add(new Tag { CategoryKey = "SPO", Name = "olympiad" });
        Tags.Add(new Tag { CategoryKey = "SPO", Name = "train for: {major sports}" });
    }
    public void PopulateUsers()
    {
        Users.Add(new User { Username = "tobi" });
        Users.Add(new User { Username = "Tobi" });
        Users.Add(new User { Username = "theDad" });
        Users.Add(new User { Username = "thePro" });
        Users.Add(new User { Username = "theNiceOne" });
        Users.Add(new User { Username = "theCriticalOne" });
        Users.Add(new User { Username = "theInspiredOne" });
        Users.Add(new User { Username = "prefa" });
    }
    public void PopulateCups()
    {
        foreach (var user in Users)
        {
            foreach (var category in Cats)
            {
                Cups.Add(new()
                {
                    CategoryKey = category.Key,
                    Username = user.Username,
                    Value = 0
                });
            }
            foreach (var tag in Tags)
            {
                Cups.Add(new()
                {
                    CategoryKey = tag.CategoryKey,
                    Username = user.Username,
                    TagName = tag.Name,
                    Value = 0
                });
            }
        }
    }
    public void PopulateIdeas()
    {
        string tWorld = "w/orld method";
        string tEasyW = "EasyWipe";
        Ideas.Add(new()
        {
            Title = tWorld,
            Slogan = "Apply Capitalism Against Global Warming",
            CreatorName = "tobi",
            Tags = new()
                {
                    Tags.First(t => t.CategoryKey=="ENV" && t.Name=="CO2 reduction"),
                    Tags.First(t => t.CategoryKey=="SOC" && t.Name=="grand movement"),
                }
        });
        Ideas.Add(new()
        {
            Title = tEasyW,
            Slogan = "No More Wiping Pain (for men)",
            CreatorName = "tobi",
            Tags = new()
                {
                    Tags.First(t => t.CategoryKey=="DSGN" && t.Name=="autarkic solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Integrated Fridge",
            Slogan = "Use the Fridge's Heat For Warm Water",
            CreatorName = "theDad",
            Tags = new List<Tag>
                {
                    Tags.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                }
        });
        Ideas.Add(new()
        {
            Title = "LookDown Mirror",
            Slogan = "IR Sensor in Car Side Mirror Checks Ice",
            CreatorName = "theDad",
            Tags = new()
                {
                    Tags.First(t => t.CategoryKey == "TRA" && t.Name == "cars"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Contra Soleil",
            Slogan = "Simple Styrofoam w/ Sucker on Window to Block Sun",
            CreatorName = "theDad",
            Tags = new()
                {
                    Tags.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                    Tags.First(t => t.CategoryKey=="HOME" && t.Name=="living solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "IntegratedPV",
            Slogan = "Roof Tile that is Also a Solar Collector",
            CreatorName = "prefa",
            Tags = new()
                {
                    Tags.First(t => t.CategoryKey == "POW" && t.Name == "alternatives"),
                    Tags.First(t => t.CategoryKey == "DSGN" && t.Name == "electronic devices"),
                }
        });
        //for (int i = 1; i <= 50; i++)
        //{
        //    Ideas.Add(new()
        //    {
        //        Title = "DummyIdea" + i,
        //        Slogan = "Some slogan.",
        //        CreatorName = "tobi",
        //    });
        //}

    }
}
