﻿// <auto-generated />
using System;
using Blog_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Blog_Project.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20190807122807_initial")]
    partial class initial
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid[]>("ChildrenId");

                    b.Property<string>("Name");

                    b.Property<Guid>("ParentId");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Blog_Project.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LikeCount");

                    b.Property<string>("OwnerId");

                    b.Property<string>("PostId");

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Blog_Project.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid[]>("Comments");

                    b.Property<string>("Content");

                    b.Property<string>("LastUpdateDate");

                    b.Property<Guid>("LikedPosts");

                    b.Property<Guid[]>("LikedUsers");

                    b.Property<Guid>("NextPost");

                    b.Property<Guid>("OwnerId");

                    b.Property<Guid>("PreviousPost");

                    b.Property<Guid[]>("RelatedCategories");

                    b.Property<string>("SubmitDate");

                    b.Property<string>("Title");

                    b.Property<int>("ViewCount");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Blog_Project.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BirthDate");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("FacebookLink");

                    b.Property<Guid[]>("Followers");

                    b.Property<Guid[]>("Followings");

                    b.Property<string>("InstagramLink");

                    b.Property<Guid[]>("InterestedCategories");

                    b.Property<string>("LinkedinLink");

                    b.Property<Guid[]>("Posts");

                    b.Property<string>("RegistrationDate");

                    b.Property<string>("Theme");

                    b.Property<string>("TwitterLink");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
