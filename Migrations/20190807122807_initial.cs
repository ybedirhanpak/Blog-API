using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Project.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: false),
                    ChildrenId = table.Column<Guid[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    PostId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    LikeCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Comments = table.Column<Guid[]>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    RelatedCategories = table.Column<Guid[]>(nullable: true),
                    SubmitDate = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<string>(nullable: true),
                    LikedUsers = table.Column<Guid[]>(nullable: true),
                    PreviousPost = table.Column<Guid>(nullable: false),
                    NextPost = table.Column<Guid>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    LikedPosts = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Posts = table.Column<Guid[]>(nullable: true),
                    Followings = table.Column<Guid[]>(nullable: true),
                    Followers = table.Column<Guid[]>(nullable: true),
                    BirthDate = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    InterestedCategories = table.Column<Guid[]>(nullable: true),
                    Theme = table.Column<string>(nullable: true),
                    FacebookLink = table.Column<string>(nullable: true),
                    TwitterLink = table.Column<string>(nullable: true),
                    InstagramLink = table.Column<string>(nullable: true),
                    LinkedinLink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
