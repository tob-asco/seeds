using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using seeds.Dal.Model;

namespace seeds.Api.Data
{
    public class seedsApiContext : DbContext
    {
        public seedsApiContext (DbContextOptions<seedsApiContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //applying my Configuration classes
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new IdeaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryUserPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new UserIdeaInteractionConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Idea> Idea { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Category> Category { get; set; } = default!;
        public DbSet<seeds.Dal.Model.CategoryUserPreference> CategoryUserPreference { get; set; } = default!;
        public DbSet<seeds.Dal.Model.UserIdeaInteraction> UserIdeaInteraction { get; set; } = default!;
    }
}
