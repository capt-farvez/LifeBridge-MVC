using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifeBridge.Migrations
{
    /// <inheritdoc />
    public partial class UpadateUserImplementBloodDonationOrganDonation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AvailableToDonateBlood",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PublicPhoneNumber",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BloodDonationRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AmountDonatedMl = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodDonationRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BloodDonationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bloodgroup = table.Column<string>(type: "TEXT", nullable: false),
                    Perpose = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BloodDonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BloodDonationRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganDonationRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OrganTypeDonated = table.Column<int>(type: "INTEGER", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganDonationRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganDonationRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganDonationRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Bloodgroup = table.Column<string>(type: "TEXT", nullable: false),
                    OrganType = table.Column<string>(type: "TEXT", nullable: false),
                    Perpose = table.Column<string>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Phone = table.Column<string>(type: "TEXT", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganDonationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganDonationRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationRecords_UserId",
                table: "BloodDonationRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BloodDonationRequests_UserId",
                table: "BloodDonationRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganDonationRecords_UserId",
                table: "OrganDonationRecords",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganDonationRequests_UserId",
                table: "OrganDonationRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BloodDonationRecords");

            migrationBuilder.DropTable(
                name: "BloodDonationRequests");

            migrationBuilder.DropTable(
                name: "OrganDonationRecords");

            migrationBuilder.DropTable(
                name: "OrganDonationRequests");

            migrationBuilder.DropColumn(
                name: "AvailableToDonateBlood",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PublicPhoneNumber",
                table: "Users");
        }
    }
}
