using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;
using System.Reflection.Emit;

namespace seeds.Api.Data;

/* proposed by GPT */

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        /* auto-generate the id
         *builder.HasKey(u => u.Id);
         *builder.Property(u => u.Id)
         *   .ValueGeneratedOnAdd();
         */

        //add uniqueness constraints
        builder.HasIndex(u => u.Username)
            .IsUnique();
        builder.HasIndex(u => u.Email)
            .IsUnique();

        //nullable fields
        builder.Property(u => u.Password)
            .IsRequired(false);
        builder.Property(u => u.Email)
            .IsRequired(false);

        // Other property configurations...
    }
}
