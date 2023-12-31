﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seeds.Dal.Model;

namespace seeds.Api.Data;

public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        #region Relations
        // Idea : Presentation = 1 : 1
        builder.HasOne(i => i.Presentation)
            .WithOne()
            .HasForeignKey<Presentation>(p => p.IdeaId)
            .IsRequired(true);

        // User : Idea = 1 : N (maybe make N:M for idea collaborations)
        builder.HasOne(i => i.Creator)
            .WithMany(u => u.CreatedIdeas)
            .HasForeignKey(i => i.CreatorName)
            .IsRequired(true);

        // Idea : Topic = M : N
        builder.HasMany(i => i.Topics)
            .WithMany(t => t.Ideas)
            .UsingEntity<IdeaTopic>();

        /* User : Idea = M : N (UserIdeaInteraction)
         * setup in UserConfiguration
         */
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
