using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementAppRepositoryLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Create Users table ────────────────────────────────────────────
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id           = table.Column<int>(nullable: false)
                                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email        = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName    = table.Column<string>(maxLength: 100, nullable: true),
                    LastName     = table.Column<string>(maxLength: 100, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    GoogleId     = table.Column<string>(maxLength: 200, nullable: true),
                    Role         = table.Column<string>(maxLength: 20, nullable: false, defaultValue: "User"),
                    IsActive     = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedAt    = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastLoginAt  = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GoogleId",
                table: "Users",
                column: "GoogleId");

            // ── Add nullable UserId FK to QuantityMeasurements ────────────────
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "QuantityMeasurements",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_UserId",
                table: "QuantityMeasurements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuantityMeasurements_Users_UserId",
                table: "QuantityMeasurements",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuantityMeasurements_Users_UserId",
                table: "QuantityMeasurements");

            migrationBuilder.DropIndex(
                name: "IX_QuantityMeasurements_UserId",
                table: "QuantityMeasurements");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "QuantityMeasurements");

            migrationBuilder.DropTable(name: "Users");
        }
    }
}
