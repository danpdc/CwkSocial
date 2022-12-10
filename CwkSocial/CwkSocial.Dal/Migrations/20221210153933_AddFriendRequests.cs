using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CwkSocial.Dal.Migrations
{
    public partial class AddFriendRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    FriendRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiverUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateResponded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Response = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.FriendRequestId);
                    table.ForeignKey(
                        name: "FK_FriendRequests_UserProfiles_ReceiverUserProfileId",
                        column: x => x.ReceiverUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                    table.ForeignKey(
                        name: "FK_FriendRequests_UserProfiles_RequesterUserProfileId",
                        column: x => x.RequesterUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstFriendUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondFriendUserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateEstablished = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FriendshipStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_UserProfiles_FirstFriendUserProfileId",
                        column: x => x.FirstFriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                    table.ForeignKey(
                        name: "FK_Friendships_UserProfiles_SecondFriendUserProfileId",
                        column: x => x.SecondFriendUserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "UserProfileId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverUserProfileId",
                table: "FriendRequests",
                column: "ReceiverUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RequesterUserProfileId",
                table: "FriendRequests",
                column: "RequesterUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FirstFriendUserProfileId",
                table: "Friendships",
                column: "FirstFriendUserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SecondFriendUserProfileId",
                table: "Friendships",
                column: "SecondFriendUserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friendships");
        }
    }
}
