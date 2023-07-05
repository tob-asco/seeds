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

        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //applying my Configuration classes
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new IdeaConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<seeds.Dal.Model.Idea> Idea { get; set; } = default!;
    }
}
