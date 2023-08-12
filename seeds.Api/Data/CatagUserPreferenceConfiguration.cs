using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;
using System.Reflection.Emit;

namespace seeds.Api.Data;

public class CatagUserPreferenceConfiguration : IEntityTypeConfiguration<CatagUserPreference>
{
    public void Configure(EntityTypeBuilder<CatagUserPreference> builder)
    {
        builder.Property(cup => cup.Id)
            .ValueGeneratedOnAdd();

        // TagName is null if the CUP is for a category
        builder.Property(cup => cup.TagName)
            .IsRequired(false);
    }
}
