using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Project.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
