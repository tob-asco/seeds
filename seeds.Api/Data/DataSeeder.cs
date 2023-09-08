using seeds.Dal.Model;

namespace seeds.Api.Data;

public class DataSeeder
{
    private readonly seedsApiContext context;
    public List<Category> Cats = new();
    public List<Family> Fams = new();
    public List<Topic> Topics = new();
    public List<User> Users = new();
    public List<UserPreference> Cups = new();
    public List<Idea> Ideas = new();

    public DataSeeder(seedsApiContext dbContext)
    {
        context = dbContext;
    }

    public void SeedData()
    {
        PopulateCats();
        if (!context.Category.Any()) { context.Category.AddRange(Cats); }
        else { Cats = context.Category.ToList(); }

        PopulateFams();
        if (!context.Family.Any()) { context.Family.AddRange(Fams); }
        else { Fams = context.Family.ToList(); }

        PopulateTopics();
        if (!context.Topic.Any()) { context.Topic.AddRange(Topics); }
        else { Topics = context.Topic.ToList(); }

        PopulateUsers();
        if (!context.User.Any()) { context.User.AddRange(Users); }
        else { Users = context.User.ToList(); }

        PopulateIdeas();
        if (!context.Idea.Any()) { context.Idea.AddRange(Ideas); }
        else { Ideas = context.Idea.ToList(); }

        context.SaveChanges();
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

        Cats = Cats.OrderBy(c => c.Name).ToList();
    }
    public void PopulateFams()
    {
        Fams.Add(new() { CategoryKey = "SPO", Name = "moves", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "SPO", Name = "gear", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "LIT", Name = "language", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "LIT", Name = "short", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "LIT", Name = "poetry", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "MUS", Name = "local", ProbablePreference = 0 });
        Fams.Add(new() { CategoryKey = "MUS", Name = "mood", ProbablePreference = 0 });
        Fams.Add(new() { CategoryKey = "ART", Name = "movement", ProbablePreference = 0 });
        Fams.Add(new() { CategoryKey = "GOV", Name = "for city", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "GOV", Name = "for country", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "HEAL", Name = "region", ProbablePreference = 0 });
        Fams.Add(new() { CategoryKey = "FEA", Name = "operating system", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "FEA", Name = "online meeting", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "FEA", Name = "social medium", ProbablePreference = 0 });
        Fams.Add(new() { CategoryKey = "FEA", Name = "streaming provider", ProbablePreference = -1 });
        Fams.Add(new() { CategoryKey = "FEA", Name = "music app", ProbablePreference = -1 });
    }
    public void PopulateTopics()
    {
        Topics.Add(new Topic { CategoryKey = "NoC", Name = "fusion" });

        foreach (string musicApp in new List<string>() { "Spotify", "Apple Music", "Amazon Music", "Google Play Music", "YouTube Music", "Tidal", "Deezer", "Pandora", "SoundCloud", "iHeartRadio" })
            Topics.Add(new Topic
            {
                CategoryKey = "FEA",
                Name = $"music app: {musicApp}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("music app")).Id
            });
        foreach (string streamingProvider in new List<string>() { })
            Topics.Add(new Topic
            {
                CategoryKey = "FEA",
                Name = $"streaming provider: {streamingProvider}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("streaming provider")).Id

            });
        foreach (string socialMedium in new List<string>() { "Netflix", "Hulu", "Amazon Prime Video", "Disney plus", "HBO Max", "Apple TV plus", "Peacock", "YouTube TV", "Sling TV", "Vudu" })
            Topics.Add(new Topic
            {
                CategoryKey = "FEA",
                Name = $"social media: {socialMedium}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("social medium")).Id

            });
        foreach (string onlineMeetingApp in new List<string>() { "Zoom", "Microsoft Teams", "Google Meet", "Cisco Webex", "Skype", "GoToMeeting", "BlueJeans", "Slack", "Discord", "Zoho Meeting", "Adobe Connect" })
            Topics.Add(new Topic
            {
                CategoryKey = "FEA",
                Name = $"online meeting: {onlineMeetingApp}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("online meeting")).Id
            });
        foreach (string os in new List<string>() { "Windows", "macOS", "Linux", "Chrome OS", "iOS", "Android", "Unix" })
            Topics.Add(new Topic
            {
                CategoryKey = "FEA",
                Name = $"operating system: {os}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("operating system")).Id
            });
        Topics.Add(new Topic { CategoryKey = "FEA", Name = "games" });

        Topics.Add(new Topic { CategoryKey = "TRA", Name = "electro" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "alternative fuels" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "cars" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "motorbikes" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "bicycle" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "new transportation" });
        Topics.Add(new Topic { CategoryKey = "TRA", Name = "public transport" });

        Topics.Add(new Topic { CategoryKey = "POW", Name = "alternatives" });
        Topics.Add(new Topic { CategoryKey = "POW", Name = "storage" });
        Topics.Add(new Topic { CategoryKey = "POW", Name = "reduction" });

        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "gaming" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "traveling" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "beauty" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "dating" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "too much time" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "life hacks" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "series & movies" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "home plants" });
        Topics.Add(new Topic { CategoryKey = "LIFE", Name = "tinkering" }); //(= basteln)

        Topics.Add(new Topic { CategoryKey = "HOME", Name = "living solutions" });
        Topics.Add(new Topic { CategoryKey = "HOME", Name = "garden" });
        Topics.Add(new Topic { CategoryKey = "HOME", Name = "keep it clean" });

        Topics.Add(new Topic { CategoryKey = "ENV", Name = "CO2 reduction" });
        Topics.Add(new Topic { CategoryKey = "ENV", Name = "climate change" });
        Topics.Add(new Topic { CategoryKey = "ENV", Name = "extreme weather" });
        Topics.Add(new Topic { CategoryKey = "ENV", Name = "save the bees" });
        Topics.Add(new Topic { CategoryKey = "ENV", Name = "reduce plastic" });

        Topics.Add(new Topic { CategoryKey = "CPU", Name = "games" });
        Topics.Add(new Topic { CategoryKey = "CPU", Name = "mobile apps" });
        Topics.Add(new Topic { CategoryKey = "CPU", Name = "databases" });
        Topics.Add(new Topic { CategoryKey = "CPU", Name = "server" });
        Topics.Add(new Topic { CategoryKey = "CPU", Name = "new languages" });
        Topics.Add(new Topic { CategoryKey = "CPU", Name = "hacking / security" });

        Topics.Add(new Topic { CategoryKey = "H4H", Name = "diminishing inequality" });
        Topics.Add(new Topic { CategoryKey = "H4H", Name = "food/water supply" });
        Topics.Add(new Topic { CategoryKey = "H4H", Name = "power supply" });
        Topics.Add(new Topic { CategoryKey = "H4H", Name = "contra unemployment" });

        Topics.Add(new Topic { CategoryKey = "EDU", Name = "teaching style" });
        Topics.Add(new Topic { CategoryKey = "EDU", Name = "educational system" });
        Topics.Add(new Topic { CategoryKey = "EDU", Name = "early development" });

        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "coffee" });
        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "tea" });
        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "cocktails" });
        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "secret recipes" });
        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "hospitality industry" }); //(= Gastronomie)
        Topics.Add(new Topic { CategoryKey = "FOOD", Name = "substitutes" });

        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "healing movements" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "nutrition" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "meditational" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "mental health" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "minimal workout" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "prophylactic lifestyle" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "contra addiction" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "grandma's medicine" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "exercises vs. pain" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "energize me" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "get happy!" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "posture" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "hygiene" });
        Topics.Add(new Topic { CategoryKey = "HEAL", Name = "allergies" });
        foreach (string region in new List<string>() { "eyes", "head", "shoulder", "back", "stomach", "hands" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "HEAL",
                Name = $"region: {region}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("region")).Id
            });
        }

        Topics.Add(new Topic { CategoryKey = "GOV", Name = "new law" });
        Topics.Add(new Topic { CategoryKey = "GOV", Name = "adapting law" });
        Topics.Add(new Topic { CategoryKey = "GOV", Name = "reducing law" });
        List<string> countries = new List<string>() { "Argentina", "Australia", "Austria", "Belgium", "Brazil", "Canada", "Chile", "China", "Colombia", "Egypt", "France", "Germany", "Greece", "India", "Indonesia", "Ireland", "Israel", "Italy", "Japan", "Malaysia", "Mexico", "Netherlands", "New Zealand", "Nigeria", "Pakistan", "Peru", "Philippines", "Poland", "Portugal", "Russia", "Saudi Arabia", "Singapore", "South Africa", "South Korea", "Spain", "Sweden", "Switzerland", "Thailand", "Turkey", "Ukraine", "United Arab Emirates", "UK", "USA", "Vietnam" };
        foreach (string country in countries)
        {
            Topics.Add(new Topic
            {
                CategoryKey = "GOV",
                Name = $"for country: {country}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("for country")).Id
            });
        } //(195 countries on Earth)
        foreach (string city in new List<string>() { "Hamburg", "Munich", "Salzburg", "Paris" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "GOV",
                Name = $"for city: {city}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("for city")).Id
            });
        } //(approx. 10 000 cities on Earth)

        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "furniture" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "architectural" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "stationery" }); //(= Schreibwaren)
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "electronic devices" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "haut couture" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "jewelry" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "autarkic solutions" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "shoes" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "survival" });
        Topics.Add(new Topic { CategoryKey = "DSGN", Name = "watches" });

        Topics.Add(new Topic { CategoryKey = "OUT", Name = "surviving" });
        Topics.Add(new Topic { CategoryKey = "OUT", Name = "survival gear" });
        Topics.Add(new Topic { CategoryKey = "OUT", Name = "camping" });
        Topics.Add(new Topic { CategoryKey = "OUT", Name = "backpacking" });
        Topics.Add(new Topic { CategoryKey = "OUT", Name = "tents" });
        Topics.Add(new Topic { CategoryKey = "OUT", Name = "functional wear" });

        Topics.Add(new Topic { CategoryKey = "ART", Name = "painting" });
        Topics.Add(new Topic { CategoryKey = "ART", Name = "sculpting" });
        foreach (string movement in new List<string>() { "Modernism", "Realism", "Cubism", "Avant-Garde" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "ART",
                Name = $"movement: {movement}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("movement")).Id
            });
        }

        foreach (string mood in new List<string>() { "electro", "party", "dancing", "heroic", "classic", "deep", "tense" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "MUS",
                Name = $"mood: {mood}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("mood")).Id
            });
        }
        foreach (string country in countries)
        {
            Topics.Add(new Topic
            {
                CategoryKey = "MUS",
                Name = $"local: {country}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("local")).Id
            });
        }

        Topics.Add(new Topic { CategoryKey = "LIT", Name = "novel topics" });
        Topics.Add(new Topic { CategoryKey = "LIT", Name = "sciFi topics" });
        Topics.Add(new Topic { CategoryKey = "LIT", Name = "sayings" });
        Topics.Add(new Topic { CategoryKey = "LIT", Name = "one-liners" });
        Topics.Add(new Topic { CategoryKey = "LIT", Name = "showerthoughts" });
        foreach (string lang in new List<string>() { "Arabic", "Chinese", "English", "French", "German", "Hindi", "Italian", "Japanese", "Korean", "Portuguese", "Russian", "Spanish", "Turkish", "Urdu" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "LIT",
                Name = $"poetry: {lang}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("poetry")).Id
            });
            Topics.Add(new Topic
            {
                CategoryKey = "LIT",
                Name = $"short: {lang}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("short")).Id
            });
            Topics.Add(new Topic
            {
                CategoryKey = "LIT",
                Name = $"language: {lang}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("language")).Id
            });
        }

        Topics.Add(new Topic { CategoryKey = "SOC", Name = "grand movement" });
        Topics.Add(new Topic { CategoryKey = "SOC", Name = "global crisis" });
        Topics.Add(new Topic { CategoryKey = "SOC", Name = "communities" });

        Topics.Add(new Topic { CategoryKey = "GAD", Name = "w/o electro" });
        Topics.Add(new Topic { CategoryKey = "GAD", Name = "diy" });
        Topics.Add(new Topic { CategoryKey = "GAD", Name = "3d prints" });

        foreach (string majorSport in new List<string>() { "working out", "climbing sports", "hiking", "snow sports", "soccer", "football", "baseball", "basketball", "cricket", "swimming", "dancing", "chess", "MMA disciplines", "fencing sports", "table tennis", "tennis", "golf", "ice hockey", "floorball", "w/ horse" })
        {
            Topics.Add(new Topic
            {
                CategoryKey = "SPO",
                Name = $"gear: {majorSport}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("gear")).Id
            });
            Topics.Add(new Topic
            {
                CategoryKey = "SPO",
                Name = $"moves: {majorSport}",
                FamilyId = context.Family.Local.First(f => f.Name.StartsWith("moves")).Id
            });
        }
        Topics.Add(new Topic { CategoryKey = "SPO", Name = $"olympiad" });
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
    public void PopulateIdeas()
    {
        string tWorld = "w/orld method";
        string tEasyW = "EasyWipe";
        Ideas.Add(new()
        {
            Title = tWorld,
            Slogan = "Apply Capitalism Against Global Warming",
            CreatorName = "tobi",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="ENV" && t.Name=="CO2 reduction"),
                    Topics.First(t => t.CategoryKey=="SOC" && t.Name=="grand movement"),
                }
        });
        Ideas.Add(new()
        {
            Title = tEasyW,
            Slogan = "No More Wiping Pain (for men)",
            CreatorName = "tobi",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="DSGN" && t.Name=="autarkic solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Integrated Fridge",
            Slogan = "Use the Fridge's Heat For Warm Water",
            CreatorName = "theDad",
            Topics = new List<Topic>
                {
                    Topics.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                }
        });
        Ideas.Add(new()
        {
            Title = "LookDown Mirror",
            Slogan = "IR Sensor in Car Side Mirror Checks Ice",
            CreatorName = "theDad",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey == "TRA" && t.Name == "cars"),
                }
        });
        Ideas.Add(new()
        {
            Title = "Contra Soleil",
            Slogan = "Simple Styrofoam w/ Sucker on Window to Block Sun",
            CreatorName = "theDad",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey=="POW" && t.Name=="reduction"),
                    Topics.First(t => t.CategoryKey=="HOME" && t.Name=="living solutions"),
                }
        });
        Ideas.Add(new()
        {
            Title = "IntegratedPV",
            Slogan = "Roof Tile that is Also a Solar Collector",
            CreatorName = "prefa",
            Topics = new()
                {
                    Topics.First(t => t.CategoryKey == "POW" && t.Name == "alternatives"),
                    Topics.First(t => t.CategoryKey == "DSGN" && t.Name == "electronic devices"),
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
