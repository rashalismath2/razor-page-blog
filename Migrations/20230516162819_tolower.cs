using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogSite.Migrations
{
    /// <inheritdoc />
    public partial class tolower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("09071cd4-d2c3-47d9-997a-b4dfc436b2b7"), "technology" },
                    { new Guid("a52fb7d2-d3b9-47cb-a302-4b74fbd5f596"), "machine learning" },
                    { new Guid("c54ae87d-a7b8-4040-8654-bb1089244763"), "data science" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("09071cd4-d2c3-47d9-997a-b4dfc436b2b7"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a52fb7d2-d3b9-47cb-a302-4b74fbd5f596"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c54ae87d-a7b8-4040-8654-bb1089244763"));

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
    }
}
