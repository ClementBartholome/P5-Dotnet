using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class AddVoitureCodeVin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CodeVin",
                table: "Voiture",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeVin",
                table: "Voiture");
        }
    }
}
