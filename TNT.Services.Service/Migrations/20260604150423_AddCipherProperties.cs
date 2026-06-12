using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TNT.Services.Service.Migrations
{
    /// <inheritdoc />
    public partial class AddCipherProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CipherIV",
                table: "Application",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CipherKey",
                table: "Application",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CipherIV",
                table: "Application");

            migrationBuilder.DropColumn(
                name: "CipherKey",
                table: "Application");
        }
    }
}
