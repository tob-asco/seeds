using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        #region Relation
        // Tag : Category = N : 1
        builder.HasOne(t => t.Category)
            .WithMany(c => c.Tags)
            .HasForeignKey(t => t.CategoryKey)
            .IsRequired(true);

        // tag : user = M : N (+some preference, hence the explicit entity)
        builder.HasMany(t => t.Users)
            .WithMany(u => u.Tags)
            .UsingEntity<CatagUserPreference>();

        // Tag : Family = N : 1 (N=0 is allowed)
        builder.HasOne(t => t.Family)
            .WithMany(c => c.Tags)
            .HasForeignKey(t => t.FamilyId)
            .IsRequired(false);
        #endregion

        #region Constraints
        builder.HasIndex(t => new { t.CategoryKey, t.Name })
            .IsUnique();
        #endregion
    }
}
