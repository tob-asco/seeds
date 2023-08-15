using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        #region Relations
        // cat : user = M : N (+some preference, hence the explicit entity)
        builder.HasMany(c => c.Users)
            .WithMany(u => u.Categories)
            .UsingEntity<UserPreference>();
        #endregion

        //fix the key to have only 6 letters
        builder.Property(c => c.Key)
            .HasMaxLength(6);
    }
}
