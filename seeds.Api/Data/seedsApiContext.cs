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
        {
        }

        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //add uniqueness constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //nullable fields
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired(false);

            //applying my UserConfiguration class
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<seeds.Dal.Model.Idea> Idea { get; set; } = default!;
    }
}
