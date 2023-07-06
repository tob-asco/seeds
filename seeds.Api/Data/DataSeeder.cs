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
        if (!_dbContext.User.Any())
        {
            _dbContext.User.Add(new User { Username = "tobi" });
            _dbContext.User.Add(new User { Username = "theDad" });
            _dbContext.User.Add(new User { Username = "thePro" });
            _dbContext.User.Add(new User { Username = "theNiceOne" });
            _dbContext.User.Add(new User { Username = "theCriticalOne" });
            _dbContext.User.Add(new User { Username = "theInspiredOne" });
            _dbContext.User.Add(new User { Username = "prefa" });
        }

        if (!_dbContext.Idea.Any())
        {
            _dbContext.Idea.Add(new Idea
            {
                Title = "w/orld method",
                Slogan = "Apply Capitalism Against Global Warming",
                Creator = "tobi"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "EasyWipe",
                Slogan = "No More Wiping Pain (for men)",
                Creator = "tobi"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "Integrated Fridge",
                Slogan = "Use the Fridge's Heat For Warm Water",
                Creator = "theDad"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "LookDown Mirror",
                Slogan = "IR Sensor in Car Side Mirror Checks Ice",
                Creator = "theDad"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "Contra Soleil",
                Slogan = "Simple Styrofoam w/ Sucker on Window to Block Sun",
                Creator = "theDad & tobi"
            });
            _dbContext.Idea.Add(new Idea
            {
                Title = "IntegratedPV",
                Slogan = "Roof Tile that is Also a Solar Collector",
                Creator = "Prefa"
            });
            for(int i = 1; i <= 100; i++)
            {
                _dbContext.Idea.Add(new Idea
                {
                    Title = "DummyIdea"+i,
                    Slogan = "Some slogan.",
                    Creator = "tobi"
                });
            }
        }

        _dbContext.SaveChanges();
    }
}
