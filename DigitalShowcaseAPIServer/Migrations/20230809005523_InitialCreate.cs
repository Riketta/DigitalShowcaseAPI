using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DigitalShowcaseAPIServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: false),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    IconURL = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diablo4_Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diablo4_Classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diablo4_ItemTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diablo4_ItemTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PassHash = table.Column<string>(type: "TEXT", maxLength: 172, nullable: false),
                    PassSalt = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Roles = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "VersaDebug_LotsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GUID = table.Column<string>(type: "TEXT", maxLength: 36, nullable: false),
                    GemLevel = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VersaDebug_LotsData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diablo4_LotsData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 128, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 300, nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diablo4_LotsData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diablo4_LotsData_Diablo4_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Diablo4_Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Diablo4_LotsData_Diablo4_ItemTypes_ItemTypeId",
                        column: x => x.ItemTypeId,
                        principalTable: "Diablo4_ItemTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UploadedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.UniqueConstraint("AK_Files_Name", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Files_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsSold = table.Column<bool>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Amount = table.Column<uint>(type: "INTEGER", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    AddedByUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateSold = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ImageURL = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    VersaDebug_LotDataId = table.Column<int>(type: "INTEGER", nullable: true),
                    Diablo4_LotDataId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lots_Diablo4_LotsData_Diablo4_LotDataId",
                        column: x => x.Diablo4_LotDataId,
                        principalTable: "Diablo4_LotsData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lots_Users_AddedByUserId",
                        column: x => x.AddedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lots_VersaDebug_LotsData_VersaDebug_LotDataId",
                        column: x => x.VersaDebug_LotDataId,
                        principalTable: "VersaDebug_LotsData",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "IconURL", "IsVisible", "Name", "Priority" },
                values: new object[,]
                {
                    { 1, "", true, "Diablo IV", 0 },
                    { 500, "", false, "VersaDebug", -1 }
                });

            migrationBuilder.InsertData(
                table: "Diablo4_Classes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Barbarian" },
                    { 2, "Druid" },
                    { 3, "Necromancer" },
                    { 4, "Rogue" },
                    { 5, "Sorcerer" }
                });

            migrationBuilder.InsertData(
                table: "Diablo4_ItemTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Helm" },
                    { 2, "Chest" },
                    { 3, "Gloves" },
                    { 4, "Pants" },
                    { 5, "Boots" },
                    { 6, "Amulet" },
                    { 7, "Ring" },
                    { 8, "Axe" },
                    { 9, "Mace" },
                    { 10, "Sword" },
                    { 11, "Dagger" },
                    { 12, "Scythe" },
                    { 13, "Wand" },
                    { 14, "Bow" },
                    { 15, "Crossbow" },
                    { 16, "Staff" },
                    { 17, "Polearm" },
                    { 18, "Two-Handed Axe" },
                    { 19, "Two-Handed Mace" },
                    { 20, "Two-Handed Sword" },
                    { 21, "Two-Handed Scythe" },
                    { 22, "Shield" },
                    { 23, "Totem" },
                    { 24, "Focus" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateCreated", "Name", "PassHash", "PassSalt", "Roles" },
                values: new object[] { -1, new DateTime(2003, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "DebugUser", "Hash", "Salt", 0 });

            migrationBuilder.InsertData(
                table: "VersaDebug_LotsData",
                columns: new[] { "Id", "GUID", "GemLevel" },
                values: new object[,]
                {
                    { 1, "GUID-0010", 10 },
                    { 2, "GUID-0020", 20 },
                    { 3, "GUID-0030", 30 }
                });

            migrationBuilder.InsertData(
                table: "Lots",
                columns: new[] { "Id", "AddedByUserId", "Amount", "CategoryId", "DateAdded", "DateSold", "Diablo4_LotDataId", "ImageURL", "IsSold", "Price", "Priority", "VersaDebug_LotDataId" },
                values: new object[,]
                {
                    { 1, -1, 0u, 500, new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2722), new DateTime(2024, 12, 21, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2733), null, "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png", false, -1m, 5, 1 },
                    { 2, -1, 0u, 500, new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2741), new DateTime(2026, 1, 25, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2741), null, "https://www.minecraft.net/content/dam/games/minecraft/logos/logo-minecraft.svg", true, -1m, 3, 2 },
                    { 3, -1, 0u, 500, new DateTime(2023, 8, 9, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2743), new DateTime(2026, 11, 21, 3, 55, 22, 930, DateTimeKind.Local).AddTicks(2744), null, "https://cdn.sstatic.net/Img/teams/teams-illo-free-sidebar-promo.svg", false, -1m, 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Diablo4_LotsData_ClassId",
                table: "Diablo4_LotsData",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Diablo4_LotsData_ItemTypeId",
                table: "Diablo4_LotsData",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_UploadedByUserId",
                table: "Files",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_AddedByUserId",
                table: "Lots",
                column: "AddedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_Diablo4_LotDataId",
                table: "Lots",
                column: "Diablo4_LotDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_VersaDebug_LotDataId",
                table: "Lots",
                column: "VersaDebug_LotDataId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Lots");

            migrationBuilder.DropTable(
                name: "Diablo4_LotsData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VersaDebug_LotsData");

            migrationBuilder.DropTable(
                name: "Diablo4_Classes");

            migrationBuilder.DropTable(
                name: "Diablo4_ItemTypes");
        }
    }
}
