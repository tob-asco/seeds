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
    [Migration("20230802131036_add_entity_topic_and_use_cup_as_join_entity")]
    partial class add_entity_topic_and_use_cup_as_join_entity
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

            modelBuilder.Entity("seeds.Dal.Model.CatopicUserPreference", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("CategoryKey")
                        .IsRequired()
                        .HasColumnType("character varying(3)")
                        .HasColumnName("category_key");

                    b.Property<string>("TagName")
                        .HasColumnType("text")
                        .HasColumnName("topic_name");

                    b.Property<string>("TagsCategoryKey")
                        .HasColumnType("character varying(3)");

                    b.Property<string>("TagsName")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("UsersUsername")
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer")
                        .HasColumnName("value");

                    b.HasKey("Id");

                    b.HasIndex("CategoryKey");

                    b.HasIndex("UsersUsername");

                    b.HasIndex("TagsCategoryKey", "TagsName");

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

                    b.HasKey("Id");

                    b.HasIndex("CategoryKey");

                    b.HasIndex("CreatorName");

                    b.ToTable("ideas");
                });

            modelBuilder.Entity("seeds.Dal.Model.Presentation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("IdeaId")
                        .HasColumnType("integer")
                        .HasColumnName("idea_id");

                    b.HasKey("Id");

                    b.HasIndex("IdeaId")
                        .IsUnique();

                    b.ToTable("presentations");
                });

            modelBuilder.Entity("seeds.Dal.Model.Tag", b =>
                {
                    b.Property<string>("CategoryKey")
                        .HasColumnType("character varying(3)")
                        .HasColumnName("category_key");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("CategoryKey", "Name");

                    b.ToTable("topics");
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

            modelBuilder.Entity("seeds.Dal.Model.CatopicUserPreference", b =>
                {
                    b.HasOne("seeds.Dal.Model.Category", null)
                        .WithMany("CatopicUserPreferences")
                        .HasForeignKey("CategoryKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("seeds.Dal.Model.User", null)
                        .WithMany("CatopicUserPreferences")
                        .HasForeignKey("UsersUsername");

                    b.HasOne("seeds.Dal.Model.Tag", null)
                        .WithMany("CatopicUserPreferences")
                        .HasForeignKey("TagsCategoryKey", "TagsName");
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

            modelBuilder.Entity("seeds.Dal.Model.Presentation", b =>
                {
                    b.HasOne("seeds.Dal.Model.Idea", null)
                        .WithOne()
                        .HasForeignKey("seeds.Dal.Model.Presentation", "IdeaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("seeds.Dal.Model.Tag", b =>
                {
                    b.HasOne("seeds.Dal.Model.Category", "Category")
                        .WithMany("Tags")
                        .HasForeignKey("CategoryKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
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
                    b.Navigation("CatopicUserPreferences");

                    b.Navigation("Ideas");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("seeds.Dal.Model.Tag", b =>
                {
                    b.Navigation("CatopicUserPreferences");
                });

            modelBuilder.Entity("seeds.Dal.Model.User", b =>
                {
                    b.Navigation("CatopicUserPreferences");

                    b.Navigation("CreatedIdeas");
                });
#pragma warning restore 612, 618
        }
    }
}
