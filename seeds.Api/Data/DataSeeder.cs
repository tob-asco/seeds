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
        categories.Add(new Category
        {
            Key = "NoC",
            Name = "No Category"
        });
        categories.Add(new Category
        {
            Key = "GAD",
            Name = "gadgets"
        });
        categories.Add(new Category
        {
            Key = "ITE",
            Name = "improve the existing"
        });
        categories.Add(new Category
        {
            Key = "ENV",
            Name = "environmental"
        });
        categories.Add(new Category
        {
            Key = "H4H",
            Name = "human for human"
        });
        categories.Add(new Category
        {
            Key = "DSN",
            Name = "design"
        });
        categories.Add(new Category
        {
            Key = "LIT",
            Name = "literature"
        });
        if (!_dbContext.Category.Any())
        {
            _dbContext.Category.AddRange(categories);
        }
        else { categories = _dbContext.Category.ToList(); }
        #endregion
        #region Tags
        tags.Add(new Tag
        {
            CategoryKey = "NoC",
            Name = "tag"
        });
        if (!_dbContext.Tag.Any())
        {
            _dbContext.Tag.AddRange(tags);
        }
        else { categories = _dbContext.Category.ToList(); }
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
