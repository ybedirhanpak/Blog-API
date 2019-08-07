using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Project.Migrations
{
    public partial class adduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Theme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
