using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        // Category : Family = 1 : N
        builder.HasOne(f => f.Category)
            .WithMany()
            .HasForeignKey(f => f.CategoryKey)
            .IsRequired(true);
    }
}
