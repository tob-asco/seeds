﻿using Microsoft.EntityFrameworkCore;
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
            modelBuilder.ApplyConfiguration(new UserPreferenceConfiguration());
            modelBuilder.ApplyConfiguration(new UserIdeaInteractionConfiguration());
            modelBuilder.ApplyConfiguration(new PresentationConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
            modelBuilder.ApplyConfiguration(new IdeaTagConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Idea> Idea { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Category> Category { get; set; } = default!;
        public DbSet<seeds.Dal.Model.UserPreference> CatagUserPreference { get; set; } = default!;
        public DbSet<seeds.Dal.Model.UserIdeaInteraction> UserIdeaInteraction { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Presentation> Presentation { get; set; } = default!;
        public DbSet<seeds.Dal.Model.Tag> Tag { get; set; } = default!;
        public DbSet<seeds.Dal.Model.IdeaTag> IdeaTag { get; set; } = default!;
    }
}
