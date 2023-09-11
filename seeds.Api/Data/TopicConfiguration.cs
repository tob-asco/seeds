using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        #region Relation
        // Topic : Category = N : 1
        builder.HasOne(t => t.Category)
            .WithMany(c => c.Topics)
            .HasForeignKey(t => t.CategoryKey)
            .IsRequired(true);

        // topic : user = M : N (+some preference, hence the explicit entity)
        builder.HasMany(t => t.Users)
            .WithMany(u => u.Topics)
            .UsingEntity<UserPreference>();

        // Topic : Family = N : 1 (N=0 is allowed)
        builder.HasOne(t => t.Family)
            .WithMany(c => c.Topics)
            .HasForeignKey(t => t.FamilyId)
            .IsRequired(false);
        #endregion

        #region Constraints
        builder.HasIndex(t => new { t.CategoryKey, t.Name })
            .IsUnique();
        #endregion
    }
}
