﻿using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;
using System.Reflection.Emit;

namespace seeds.Api.Data;

public class CatagUserPreferenceConfiguration : IEntityTypeConfiguration<CatagUserPreference>
{
    public void Configure(EntityTypeBuilder<CatagUserPreference> builder)
    {
        #region Constraints
        builder.HasIndex(cup => new { cup.CategoryKey, cup.Username, cup.TagName })
            .IsUnique();
        #endregion
        builder.Property(cup => cup.Id)
            .ValueGeneratedOnAdd();

        // TagName is null if the CUP is for a category
        builder.Property(cup => cup.TagName)
            .IsRequired(false);
    }
}
