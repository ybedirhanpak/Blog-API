using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog_Project.Migrations
{
    public partial class categoryuserupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_NextPostId",
                table: "Posts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Posts_PreviousPostId",
                table: "Posts");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PreviousPostId",
                table: "Posts",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Categories",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_NextPostId",
                table: "Posts",
                column: "NextPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Posts_NextPostId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Categories");

            migrationBuilder.AlterColumn<Guid>(
                name: "PreviousPostId",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Posts_PreviousPostId",
                table: "Posts",
                column: "PreviousPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Posts_NextPostId",
                table: "Posts",
                column: "NextPostId",
                principalTable: "Posts",
                principalColumn: "PreviousPostId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
