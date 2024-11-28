using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StallSync.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponsiblePerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TaskItems",
                columns: new[] { "Id", "Description", "EndDate", "IsCompleted", "ResponsiblePerson", "StartDate", "Title" },
                values: new object[,]
                {
                    { 1, "Släpp ut hästarna i hagen", new DateTime(2024, 11, 21, 8, 0, 0, 0, DateTimeKind.Unspecified), false, "Amanda Olsson", new DateTime(2024, 11, 21, 7, 0, 0, 0, DateTimeKind.Unspecified), "Utsläpp" },
                    { 2, "Ta in hästarna från hagen", new DateTime(2024, 11, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), false, "Sara Wigren", new DateTime(2024, 11, 21, 17, 0, 0, 0, DateTimeKind.Unspecified), "Intag" },
                    { 3, "Fodra hästarna morgon och kväll", new DateTime(2024, 11, 21, 19, 0, 0, 0, DateTimeKind.Unspecified), true, "Nils Oscar", new DateTime(2024, 11, 21, 6, 0, 0, 0, DateTimeKind.Unspecified), "Fodring" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskItems");
        }
    }
}
