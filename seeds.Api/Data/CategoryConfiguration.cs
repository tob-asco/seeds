using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        #region Relations
        // idea : cat = N : 1
        builder.HasMany(c => c.Ideas)
            .WithOne(i => i.Category)
            .HasForeignKey(i => i.CategoryKey)
            .IsRequired(true); // i.e. N > 0

        // cat : user = M : N (+some preference, hence the explicit entity)
        builder.HasMany(c => c.Users)
            .WithMany(u => u.Categories)
            .UsingEntity<CategoryUserPreference>();
        #endregion

        //fix the key to have only 3 letters
        builder.Property(c => c.Key)
            .HasMaxLength(3);
    }
}
