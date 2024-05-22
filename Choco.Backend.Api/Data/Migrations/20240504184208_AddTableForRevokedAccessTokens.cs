using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Choco.Backend.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTableForRevokedAccessTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevokedAccessTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevokedAccessTokens", x => x.Token);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevokedAccessTokens");
        }
    }
}
