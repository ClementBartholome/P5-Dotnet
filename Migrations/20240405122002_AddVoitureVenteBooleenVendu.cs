using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class AddVoitureVenteBooleenVendu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Vendu",
                table: "VoitureVente",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vendu",
                table: "VoitureVente");
        }
    }
}
