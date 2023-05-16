using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class tagsseed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("27373c81-2459-4736-bca0-df5ebdbdb8d1"), "Data science" },
                    { new Guid("ab510494-9ed0-4585-b526-08fcc085069a"), "Technology" },
                    { new Guid("e078c990-11eb-4bdc-a664-528f241103db"), "Machine Learning" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("27373c81-2459-4736-bca0-df5ebdbdb8d1"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("ab510494-9ed0-4585-b526-08fcc085069a"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e078c990-11eb-4bdc-a664-528f241103db"));
        }
    }
}
