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
        builder.HasKey(it => new { it.IdeaId, it.TagId });
    }
}
