using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.Dal.Migrations
{
    public partial class AddUserProfileToInteraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "PostInteraction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostInteraction_UserProfileId",
                table: "PostInteraction",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteraction_UserProfiles_UserProfileId",
                table: "PostInteraction",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteraction_UserProfiles_UserProfileId",
                table: "PostInteraction");

            migrationBuilder.DropIndex(
                name: "IX_PostInteraction_UserProfileId",
                table: "PostInteraction");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "PostInteraction");
        }
    }
}
