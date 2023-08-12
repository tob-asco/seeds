﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using seeds.Api.Data;

#nullable disable

namespace seeds.Api.Migrations
{
    [DbContext(typeof(seedsApiContext))]
    [Migration("20230713113935_join_entity_user_idea_payload_interactions")]
    partial class join_entity_user_idea_payload_interactions
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("seeds.Dal.Model.Category", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(3)
                        .HasColumnType("character varying(3)")
                        .HasColumnName("key");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Key");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("seeds.Dal.Model.CatagUserPreference", b =>
                {
                    b.Property<string>("CategoryKey")
                        .HasColumnType("character varying(3)")
                        .HasColumnName("category_key");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("CategoryKey", "Username");

                    b.HasIndex("Username");

                    b.ToTable("category_user");
                });

            modelBuilder.Entity("seeds.Dal.Model.Idea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CategoryKey")
                        .IsRequired()
                        .HasColumnType("character varying(3)")
                        .HasColumnName("category_key");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_time");

                    b.Property<string>("CreatorName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("creator");

                    b.Property<string>("Slide1")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slide1");

                    b.Property<string>("Slide2")
                        .HasColumnType("text")
                        .HasColumnName("slide2");

                    b.Property<string>("Slide3")
                        .HasColumnType("text")
                        .HasColumnName("slide3");

                    b.Property<string>("Slogan")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slogan");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("Upvotes")
                        .HasColumnType("integer")
                        .HasColumnName("upvotes");

                    b.HasKey("Id");

                    b.HasIndex("CategoryKey");

                    b.HasIndex("CreatorName");

                    b.ToTable("ideas");
                });

            modelBuilder.Entity("seeds.Dal.Model.User", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("Username");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("seeds.Dal.Model.UserIdeaInteraction", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<int>("IdeaId")
                        .HasColumnType("integer")
                        .HasColumnName("idea_id");

                    b.Property<bool>("Downvoted")
                        .HasColumnType("boolean")
                        .HasColumnName("downvoted");

                    b.Property<bool>("Upvoted")
                        .HasColumnType("boolean")
                        .HasColumnName("upvoted");

                    b.HasKey("Username", "IdeaId");

                    b.HasIndex("IdeaId");

                    b.ToTable("user_idea");
                });

            modelBuilder.Entity("seeds.Dal.Model.CatagUserPreference", b =>
                {
                    b.HasOne("seeds.Dal.Model.Category", null)
                        .WithMany("CatagUserPreferences")
                        .HasForeignKey("CategoryKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seeds.Dal.Model.User", null)
                        .WithMany("CatagUserPreferences")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("seeds.Dal.Model.Idea", b =>
                {
                    b.HasOne("seeds.Dal.Model.Category", "Category")
                        .WithMany("Ideas")
                        .HasForeignKey("CategoryKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seeds.Dal.Model.User", "Creator")
                        .WithMany("CreatedIdeas")
                        .HasForeignKey("CreatorName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("seeds.Dal.Model.UserIdeaInteraction", b =>
                {
                    b.HasOne("seeds.Dal.Model.Idea", null)
                        .WithMany()
                        .HasForeignKey("IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seeds.Dal.Model.User", null)
                        .WithMany()
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("seeds.Dal.Model.Category", b =>
                {
                    b.Navigation("CatagUserPreferences");

                    b.Navigation("Ideas");
                });

            modelBuilder.Entity("seeds.Dal.Model.User", b =>
                {
                    b.Navigation("CatagUserPreferences");

                    b.Navigation("CreatedIdeas");
                });
#pragma warning restore 612, 618
        }
    }
}
