using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class UserIdeaInteractionConfiguration : IEntityTypeConfiguration<UserIdeaInteraction>
{
    public void Configure(EntityTypeBuilder<UserIdeaInteraction> builder)
    {
        builder.HasKey(uii => new { uii.Username, uii.IdeaId });
    }
}
