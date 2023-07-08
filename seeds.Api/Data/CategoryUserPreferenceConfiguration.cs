using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class CategoryUserPreferenceConfiguration : IEntityTypeConfiguration<CategoryUserPreference>
{
    public void Configure(EntityTypeBuilder<CategoryUserPreference> builder)
    {
        builder.HasKey(cp => new {cp.CategoryKey, cp.Username});
    }
}
