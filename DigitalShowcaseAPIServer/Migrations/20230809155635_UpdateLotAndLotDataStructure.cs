using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalShowcaseAPIServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLotAndLotDataStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Diablo4_LotsData");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Diablo4_LotsData");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Lots",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Lots",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdded", "DateSold", "Description", "Name" },
                values: new object[] { new DateTime(2023, 8, 9, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6615), new DateTime(2024, 12, 21, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6624), null, null });

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateAdded", "DateSold", "Description", "Name" },
                values: new object[] { new DateTime(2023, 8, 9, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6635), new DateTime(2026, 1, 25, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6635), null, null });

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateAdded", "DateSold", "Description", "Name" },
                values: new object[] { new DateTime(2023, 8, 9, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6638), new DateTime(2026, 11, 21, 18, 56, 35, 271, DateTimeKind.Local).AddTicks(6638), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Lots");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Diablo4_LotsData",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Diablo4_LotsData",
                type: "TEXT",
                maxLength: 128,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateAdded", "DateSold" },
                values: new object[] { new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2722), new DateTime(2024, 12, 21, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2733) });

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateAdded", "DateSold" },
                values: new object[] { new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2741), new DateTime(2026, 1, 25, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2741) });

            migrationBuilder.UpdateData(
                table: "Lots",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateAdded", "DateSold" },
                values: new object[] { new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2743), new DateTime(2026, 11, 21, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2744) });
        }
    }
}
