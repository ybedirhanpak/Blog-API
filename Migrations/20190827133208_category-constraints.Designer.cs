﻿// <auto-generated />
using System;
using Blog_Project.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Blog_Project.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20190827133208_category-constraints")]
    partial class categoryconstraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Blog_Project.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl")
                        .IsRequired();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ParentId")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Blog_Project.Models.Comment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<int>("LikeCount");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<string>("PostId")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Blog_Project.Models.MainCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.HasKey("Id");

                    b.ToTable("MainCategories");
                });

            modelBuilder.Entity("Blog_Project.Models.Post", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.Property<string[]>("Tags");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("ViewCount");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Blog_Project.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BirthDate");

                    b.Property<string>("Description");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FacebookLink");

                    b.Property<string>("InstagramLink");

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("LinkedinLink");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired();

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired();

                    b.Property<string>("Role")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.Property<string>("Theme");

                    b.Property<string>("TwitterLink");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email", "UserName", "IsDeleted")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Blog_Project.Models.UserCategory", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CategoryId")
                        .IsRequired();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime>("SubmitTime");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCategory");
                });

            modelBuilder.Entity("Blog_Project.Models.UserFollow", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FollowedId")
                        .IsRequired();

                    b.Property<string>("FollowerId")
                        .IsRequired();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<DateTime>("SubmitTime");

                    b.HasKey("Id");

                    b.HasIndex("FollowedId");

                    b.HasIndex("FollowerId");

                    b.ToTable("UserFollows");
                });

            modelBuilder.Entity("Blog_Project.Models.UserLikePost", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("IsDeleted");

                    b.Property<DateTime>("LastUpdateTime");

                    b.Property<string>("PostId")
                        .IsRequired();

                    b.Property<DateTime>("SubmitTime");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLikePost");
                });

            modelBuilder.Entity("Blog_Project.Models.Category", b =>
                {
                    b.HasOne("Blog_Project.Models.MainCategory", "Parent")
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog_Project.Models.Comment", b =>
                {
                    b.HasOne("Blog_Project.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog_Project.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog_Project.Models.Post", b =>
                {
                    b.HasOne("Blog_Project.Models.Category", "Category")
                        .WithMany("RelatedPosts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog_Project.Models.User", "Owner")
                        .WithMany("Posts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog_Project.Models.UserCategory", b =>
                {
                    b.HasOne("Blog_Project.Models.Category", "Category")
                        .WithMany("FollowerUsers")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog_Project.Models.User", "User")
                        .WithMany("InterestedCategories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog_Project.Models.UserFollow", b =>
                {
                    b.HasOne("Blog_Project.Models.User", "Followed")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog_Project.Models.User", "Follower")
                        .WithMany("Followings")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog_Project.Models.UserLikePost", b =>
                {
                    b.HasOne("Blog_Project.Models.Post", "Post")
                        .WithMany("LikedUsers")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog_Project.Models.User", "User")
                        .WithMany("LikedPosts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
