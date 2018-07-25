using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductCatalogApi.Data.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CatalogBrand",
                columns: new[] { "id", "Brand" },
                values: new object[,]
                {
                    { 1, "Addidas" },
                    { 2, "Puma" },
                    { 3, "Slazenger" }
                });

            migrationBuilder.InsertData(
                table: "CatalogType",
                columns: new[] { "id", "Type" },
                values: new object[,]
                {
                    { 1, "Running" },
                    { 2, "Basketball" },
                    { 3, "Tennis" }
                });

            migrationBuilder.InsertData(
                table: "Catalog",
                columns: new[] { "Id", "CatalogBrandId", "CatalogTypeId", "Description", "Name", "PictureFileName", "PictureUrl", "Price" },
                values: new object[,]
                {
                    { 2, 2, 1, "will make you world champions", "White Line", null, "http://externalcatalogbaseurltobereplaced/api/pic/2", 88.50m },
                    { 7, 1, 1, "Light as carbon", "Deep Purple", null, "http://externalcatalogbaseurltobereplaced/api/pic/8", 238.5m },
                    { 8, 2, 1, "High Jumper", "Addidas<White> Running", null, "http://externalcatalogbaseurltobereplaced/api/pic/9", 129m },
                    { 10, 2, 1, "All round", "Inredeible", null, "http://externalcatalogbaseurltobereplaced/api/pic/11", 248.5m },
                    { 1, 3, 2, "Shoes for next century", "World Star", null, "http://externalcatalogbaseurltobereplaced/api/pic/1", 199.5m },
                    { 3, 3, 2, "You have already won gold medal", "Prism White Shoes", null, "http://externalcatalogbaseurltobereplaced/api/pic/3", 129m },
                    { 4, 2, 2, "Olympic runner", "Foundation Hitech", null, "http://externalcatalogbaseurltobereplaced/api/pic/4", 12m },
                    { 5, 1, 2, "Roslyn Red Sheet", "Roslyn White", null, "http://externalcatalogbaseurltobereplaced/api/pic/5", 188.5m },
                    { 6, 2, 2, "Lala Land", "Blue Star", null, "http://externalcatalogbaseurltobereplaced/api/pic/6", 112m },
                    { 9, 3, 2, "Dunker", "Elequent", null, "http://externalcatalogbaseurltobereplaced/api/pic/10", 12m },
                    { 11, 1, 2, "Pricesless", "London Sky", null, "http://externalcatalogbaseurltobereplaced/api/pic/12", 412m },
                    { 12, 3, 3, "Tennis Star", "Elequent", null, "http://externalcatalogbaseurltobereplaced/api/pic/13", 123m },
                    { 13, 2, 3, "Wimbeldon", "London Star", null, "http://externalcatalogbaseurltobereplaced/api/pic/14", 218.5m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Catalog",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CatalogBrand",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CatalogType",
                keyColumn: "id",
                keyValue: 3);
        }
    }
}
