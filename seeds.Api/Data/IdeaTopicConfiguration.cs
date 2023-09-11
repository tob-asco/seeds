using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class IdeaTopicConfiguration : IEntityTypeConfiguration<IdeaTopic>
{
    public void Configure(EntityTypeBuilder<IdeaTopic> builder)
    {
        // composite PK
        builder.HasKey(it => new { it.IdeaId, it.TopicId });
    }
}
