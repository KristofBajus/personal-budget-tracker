using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    IsBanned = table.Column<bool>(type: "INTEGER", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsGlobal = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Note = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Color", "CreatedAt", "DeletedAt", "IsDeleted", "IsGlobal", "Name", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, "#4CAF50", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Salary", 0, null },
                    { 2, "#8BC34A", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Freelance", 0, null },
                    { 3, "#FF9800", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Gift", 0, null },
                    { 4, "#FFC107", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Allowance", 0, null },
                    { 5, "#9E9E9E", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Other", 0, null },
                    { 6, "#FF6B6B", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Food", 1, null },
                    { 7, "#9C27B0", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Housing", 1, null },
                    { 8, "#4ECDC4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Transport", 1, null },
                    { 9, "#F44336", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Health", 1, null },
                    { 10, "#45B7D1", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Entertainment", 1, null },
                    { 11, "#E91E63", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Shopping", 1, null },
                    { 12, "#607D8B", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, "Other", 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UserId",
                table: "Categories",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
