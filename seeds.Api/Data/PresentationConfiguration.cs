using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class PresentationConfiguration : IEntityTypeConfiguration<Presentation>
{
    public void Configure(EntityTypeBuilder<Presentation> builder)
    {
        #region Relations
        // Idea : Presentation = 1 : 1
        // setup in IdeaConfiguration
        #endregion
    }
}
