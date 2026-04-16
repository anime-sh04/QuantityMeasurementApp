using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HistoryService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuantityMeasurements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OperationType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MeasurementType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FirstValue = table.Column<double>(type: "double precision", nullable: false),
                    FirstUnit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondValue = table.Column<double>(type: "double precision", nullable: false),
                    SecondUnit = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Result = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuantityMeasurements", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_MeasurementType",
                table: "QuantityMeasurements",
                column: "MeasurementType");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityMeasurements_OperationType",
                table: "QuantityMeasurements",
                column: "OperationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuantityMeasurements");
        }
    }
}
