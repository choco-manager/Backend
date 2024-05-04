using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTablesForProductsAndItsTagsWithSomePredefinedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CostPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    StockBalance = table.Column<int>(type: "integer", nullable: false),
                    IsBulk = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductTag",
                columns: table => new
                {
                    ProductsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductTag", x => new { x.ProductsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ProductProductTag_ProductTags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "ProductTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductTag_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ProductTags",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("1cb55e22-b608-4eb1-9b00-bcf99227a45b"), "Мармелад" },
                    { new Guid("3916b26d-cb71-459d-9692-2c8376ed3222"), "Брикет" },
                    { new Guid("3f9c5572-2a30-4738-ad5d-e64a78d0b1c5"), "Горький" },
                    { new Guid("6010b624-5e50-4c74-aedd-62d86968b8aa"), "С орехами" },
                    { new Guid("66978e41-810b-4e15-b271-0185365319ea"), "Паста" },
                    { new Guid("740c0d16-cff1-4fc2-9b32-198c41cd8f5b"), "Конфеты" },
                    { new Guid("800ce8c6-cda8-4a33-8232-94d1039fdcbf"), "На развес" },
                    { new Guid("8983ea6f-d617-4b68-bf35-951e987b6df2"), "Фирменный" },
                    { new Guid("ca398e3e-b307-4fb1-897d-0a879e07ed0a"), "Молочный" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductTag_TagsId",
                table: "ProductProductTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductTag");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
