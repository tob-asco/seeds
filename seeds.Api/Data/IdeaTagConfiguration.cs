using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class IdeaTagConfiguration : IEntityTypeConfiguration<IdeaTag>
{
    public void Configure(EntityTypeBuilder<IdeaTag> builder)
    {
        // composite PK
        builder.HasKey(it => new { it.IdeaId, it.CategoryKey, it.TagName });

        // Idea : Tag = M : N
        builder.HasOne<Idea>()
            .WithMany()
            .HasForeignKey(it => it.IdeaId)
            .IsRequired(true);
        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(it => new { it.CategoryKey, it.TagName })
            .IsRequired(true);
    }
}
