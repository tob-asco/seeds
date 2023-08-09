using seeds.Dal.Model;

namespace seeds.Api.Data;

public class DataSeeder
{
    private readonly seedsApiContext _dbContext;

    public DataSeeder(seedsApiContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void SeedData()
    {
        // create lists to have entries it before
        // saving the changes to the DB
        List<Category> categories = new();
        List<Tag> tags = new();
        List<User> users = new();

        #region Categories
        categories.Add(new Category { Key = "NoC", Name = "No Category" });
        categories.Add(new Category { Key = "FEA", Name = "Features" });
        categories.Add(new Category { Key = "TRA", Name = "Transportation" });
        categories.Add(new Category { Key = "POW", Name = "Energy" });
        categories.Add(new Category { Key = "LIFE", Name = "Lifestyle" });
        categories.Add(new Category { Key = "HOME", Name = "Living" });
        categories.Add(new Category { Key = "ENV", Name = "Environment" });
        categories.Add(new Category { Key = "CPU", Name = "Software" });
        categories.Add(new Category { Key = "H4H", Name = "Human for Human" });
        categories.Add(new Category { Key = "EDU", Name = "Education" });
        categories.Add(new Category { Key = "FOOD", Name = "Food & Drinks" });
        categories.Add(new Category { Key = "HEAL", Name = "Health" });
        categories.Add(new Category { Key = "NEER", Name = "Engineering" });
        categories.Add(new Category { Key = "GOV", Name = "Governmental" });
        categories.Add(new Category { Key = "DSGN", Name = "Design" });
        categories.Add(new Category { Key = "OUT", Name = "Outdoors" });
        categories.Add(new Category { Key = "ART", Name = "Arts" });
        categories.Add(new Category { Key = "MUS", Name = "Music" });
        categories.Add(new Category { Key = "LIT", Name = "Literature" });
        categories.Add(new Category { Key = "SOC", Name = "Society" });
        categories.Add(new Category { Key = "GAD", Name = "Gadgets" });
        categories.Add(new Category { Key = "SPO", Name = "Sport" });

        categories.Add(new Category { Key = "FUS", Name = "Fusion" });
        categories.Add(new Category { Key = "SCI", Name = "Science" });
        categories.Add(new Category { Key = "???", Name = "Idea Needed" });
        categories.Add(new Category { Key = "SEEDS", Name = "Internal" });
        if (!_dbContext.Category.Any()) { _dbContext.Category.AddRange(categories); }
        else { categories = _dbContext.Category.ToList(); }
        #endregion
        #region Tags
        foreach(var cat in categories)
        {
            tags.Add(new Tag { CategoryKey = cat.Key, Name = "*" });
        }
        tags.Add(new Tag { CategoryKey = "NoC", Name = "no tag" });

        tags.Add(new Tag { CategoryKey = "FEA", Name = "music app: {main music streaming apps}" });
        tags.Add(new Tag { CategoryKey = "FEA", Name = "streaming provider: {main video streaming providers}" });
        tags.Add(new Tag { CategoryKey = "FEA", Name = "social media: {main social platforms}" });
        tags.Add(new Tag { CategoryKey = "FEA", Name = "online meeting: {main meeting providers}" });
        tags.Add(new Tag { CategoryKey = "FEA", Name = "operating systems: {Win, Linux, OsX, Android, iOs, and smaller mobile Os}" });
        tags.Add(new Tag { CategoryKey = "FEA", Name = "games" });

        tags.Add(new Tag { CategoryKey = "TRA", Name = "electro" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "alternative fuels" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "cars" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "motorbikes" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "bicycle" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "new transportation" });
        tags.Add(new Tag { CategoryKey = "TRA", Name = "public transport" });

        tags.Add(new Tag { CategoryKey = "POW", Name = "alternatives" });
        tags.Add(new Tag { CategoryKey = "POW", Name = "storage" });
        tags.Add(new Tag { CategoryKey = "POW", Name = "reduction" });

        tags.Add(new Tag { CategoryKey = "LIFE", Name = "gaming" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "traveling" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "beauty" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "dating" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "too much time" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "life hacks" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "series & movies" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "home plants" });
        tags.Add(new Tag { CategoryKey = "LIFE", Name = "tinkering" }); //(= basteln)

        tags.Add(new Tag { CategoryKey = "HOME", Name = "living solutions" });
        tags.Add(new Tag { CategoryKey = "HOME", Name = "garden" });
        tags.Add(new Tag { CategoryKey = "HOME", Name = "keep it clean" });

        tags.Add(new Tag { CategoryKey = "ENV", Name = "CO2 reduction" });
        tags.Add(new Tag { CategoryKey = "ENV", Name = "climate change" });
        tags.Add(new Tag { CategoryKey = "ENV", Name = "extreme weather" });
        tags.Add(new Tag { CategoryKey = "ENV", Name = "save the bees" });
        tags.Add(new Tag { CategoryKey = "ENV", Name = "reduce plastic" });

        tags.Add(new Tag { CategoryKey = "CPU", Name = "games" });
        tags.Add(new Tag { CategoryKey = "CPU", Name = "mobile apps" });
        tags.Add(new Tag { CategoryKey = "CPU", Name = "databases" });
        tags.Add(new Tag { CategoryKey = "CPU", Name = "server" });
        tags.Add(new Tag { CategoryKey = "CPU", Name = "new languages" });
        tags.Add(new Tag { CategoryKey = "CPU", Name = "hacking / security" });

        tags.Add(new Tag { CategoryKey = "H4H", Name = "diminishing inequality" });
        tags.Add(new Tag { CategoryKey = "H4H", Name = "food/water supply" });
        tags.Add(new Tag { CategoryKey = "H4H", Name = "power supply" });
        tags.Add(new Tag { CategoryKey = "H4H", Name = "contra unemployment" });

        tags.Add(new Tag { CategoryKey = "EDU", Name = "teaching style" });
        tags.Add(new Tag { CategoryKey = "EDU", Name = "educational system" });
        tags.Add(new Tag { CategoryKey = "EDU", Name = "early development" });

        tags.Add(new Tag { CategoryKey = "FOOD", Name = "coffee" });
        tags.Add(new Tag { CategoryKey = "FOOD", Name = "tea" });
        tags.Add(new Tag { CategoryKey = "FOOD", Name = "cocktails" });
        tags.Add(new Tag { CategoryKey = "FOOD", Name = "secret recipes" });
        tags.Add(new Tag { CategoryKey = "FOOD", Name = "hospitality industry" }); //(= Gastronomie)
        tags.Add(new Tag { CategoryKey = "FOOD", Name = "substitutes" });

        tags.Add(new Tag { CategoryKey = "HEAL", Name = "healing movements" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "nutrition" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "meditational" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "mental health" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "minimal workout" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "prophylactic lifestyle" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "contra addiction" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "grandma's medicine" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "exercises vs. pain" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "energize me" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "get happy!" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "posture" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "hygiene" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "allergies" });
        tags.Add(new Tag { CategoryKey = "HEAL", Name = "body region: {major regions like eyes, head, shoulder, back, stomach, hands, ..}" });

        tags.Add(new Tag { CategoryKey = "GOV", Name = "new law" });
        tags.Add(new Tag { CategoryKey = "GOV", Name = "adapting law" });
        tags.Add(new Tag { CategoryKey = "GOV", Name = "reducing law" });
        tags.Add(new Tag { CategoryKey = "GOV", Name = "for country: {all countries}" }); //(195 countries on Earth)
        tags.Add(new Tag { CategoryKey = "GOV", Name = "for city: {all cities}" }); //(approx. 10 000 cities on Earth)

        tags.Add(new Tag { CategoryKey = "DSGN", Name = "furniture" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "architectural" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "stationery" }); //(= Schreibwaren)
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "electronic devices" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "haut couture" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "jewelry" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "autarkic solutions" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "shoes" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "survival" });
        tags.Add(new Tag { CategoryKey = "DSGN", Name = "watches" });

        tags.Add(new Tag { CategoryKey = "OUT", Name = "surviving" });
        tags.Add(new Tag { CategoryKey = "OUT", Name = "survival gear" });
        tags.Add(new Tag { CategoryKey = "OUT", Name = "camping" });
        tags.Add(new Tag { CategoryKey = "OUT", Name = "backpacking" });
        tags.Add(new Tag { CategoryKey = "OUT", Name = "tents" });
        tags.Add(new Tag { CategoryKey = "OUT", Name = "functional wear" });

        tags.Add(new Tag { CategoryKey = "ART", Name = "painting" });
        tags.Add(new Tag { CategoryKey = "ART", Name = "sculpting" });
        tags.Add(new Tag { CategoryKey = "ART", Name = "movement: {major movements like Modernism, Realism, Cubism, Avant-Garde, ...}" });

        tags.Add(new Tag { CategoryKey = "MUS", Name = "mood: {electro, party, dancing, heroic, classic, deep, tense, ...}" });
        tags.Add(new Tag { CategoryKey = "MUS", Name = "local: {all countries}" });

        tags.Add(new Tag { CategoryKey = "LIT", Name = "poetry: {all languages, maybe also dialects}" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "short: {all languages, maybe also dialects}" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "novel topics" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "sciFi topics" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "sayings" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "one-liners" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "showerthoughts" });
        tags.Add(new Tag { CategoryKey = "LIT", Name = "for language: {all languages and dialects}" });

        tags.Add(new Tag { CategoryKey = "SOC", Name = "grand movement" });
        tags.Add(new Tag { CategoryKey = "SOC", Name = "global crisis" });
        tags.Add(new Tag { CategoryKey = "SOC", Name = "communities" });

        tags.Add(new Tag { CategoryKey = "GAD", Name = "w/o electro" });
        tags.Add(new Tag { CategoryKey = "GAD", Name = "diy" });
        tags.Add(new Tag { CategoryKey = "GAD", Name = "3d prints" });

        tags.Add(new Tag { CategoryKey = "SPO", Name = "sports gear: {major sports}" });
        tags.Add(new Tag { CategoryKey = "SPO", Name = "new moves: {major sports}" });
        tags.Add(new Tag { CategoryKey = "SPO", Name = "olympiad" });
        tags.Add(new Tag { CategoryKey = "SPO", Name = "train for: {major sports}" });

        if (!_dbContext.Tag.Any()) { _dbContext.Tag.AddRange(tags); }
        else { tags = _dbContext.Tag.ToList(); }
        #endregion
        #region Users
        users.Add(new User { Username = "tobi" });
        users.Add(new User { Username = "Tobi" });
        users.Add(new User { Username = "theDad" });
        users.Add(new User { Username = "thePro" });
        users.Add(new User { Username = "theNiceOne" });
        users.Add(new User { Username = "theCriticalOne" });
        users.Add(new User { Username = "theInspiredOne" });
        users.Add(new User { Username = "prefa" });
        if (!_dbContext.User.Any())
        {
            users.AddRange(users);
        }
        #endregion
        #region Category User Preferences
        //categories and users are ready, so we can seed their relation:
        if (!_dbContext.CategoryUserPreference.Any())
        {
            foreach (var user in users)
            {
                foreach (var category in categories)
                {
                    _dbContext.CategoryUserPreference.Add(new CategoryUserPreference
                    {
                        CategoryKey = category.Key,
                        Username = user.Username,
                        Value = 0
                    });
                }
                foreach (var tag in tags)
                {
                    _dbContext.CategoryUserPreference.Add(new CategoryUserPreference
                    {
                        CategoryKey = tag.CategoryKey,
                        Username = user.Username,
                        TagName = tag.Name,
                        Value = 0
                    });
                }
            }
        }
        #endregion
        #region Ideas
        if (!_dbContext.Idea.Any())
        {
            _dbContext.Idea.Add(new Idea
            {
                Title = "w/orld method",
                Slogan = "Apply Capitalism Against Global Warming",
                CreatorName = "tobi",
                CategoryKey = "ENV"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "EasyWipe",
                Slogan = "No More Wiping Pain (for men)",
                CreatorName = "tobi",
                CategoryKey = "GAD"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "Integrated Fridge",
                Slogan = "Use the Fridge's Heat For Warm Water",
                CreatorName = "theDad",
                CategoryKey = "ITE"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "LookDown Mirror",
                Slogan = "IR Sensor in Car Side Mirror Checks Ice",
                CreatorName = "theDad",
                CategoryKey = "ITE"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "Contra Soleil",
                Slogan = "Simple Styrofoam w/ Sucker on Window to Block Sun",
                CreatorName = "theDad",
                CategoryKey = "GAD"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "IntegratedPV",
                Slogan = "Roof Tile that is Also a Solar Collector",
                CreatorName = "prefa",
                CategoryKey = "ITE"
            });
            for (int i = 1; i <= 100; i++)
            {
                _dbContext.Idea.Add(new Idea
                {
                    Title = "DummyIdea" + i,
                    Slogan = "Some slogan.",
                    CreatorName = "tobi",
                    CategoryKey = "NoC"
                });
            }
        }
        #endregion

        _dbContext.SaveChanges();
    }
}
