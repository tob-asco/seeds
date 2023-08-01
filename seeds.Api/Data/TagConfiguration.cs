using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        // Tag : Category = N : 1
        builder.HasOne(t => t.Category)
            .WithMany(c => c.Tags)
            .HasForeignKey(t => t.CategoryKey)
            .IsRequired(true);

        /* Set Tag's PK to be made from the pair
         * ( Category's key, Tag's name)
         */
        builder.HasKey(t => new { t.CategoryKey, t.Name });
    }
}
