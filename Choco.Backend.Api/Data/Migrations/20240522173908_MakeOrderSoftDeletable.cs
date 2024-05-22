using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Choco.Backend.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeOrderSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IdDeleted",
                table: "Orders",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdDeleted",
                table: "Orders");
        }
    }
}
