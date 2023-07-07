using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        #region Relations
        // idea : cat = N : 1
        // setup in CategoryConfiguration
        #endregion

        //auto-generate the id
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        //nullable fields
        builder.Property(i => i.Slide2)
            .IsRequired(false);
        builder.Property(i => i.Slide3)
            .IsRequired(false);

        //make sure the DB DateTime has no time zone
        builder.Property(i => i.CreationTime)
            .HasColumnType("timestamp without time zone");

        //uniqueness constraints...
    }
}
