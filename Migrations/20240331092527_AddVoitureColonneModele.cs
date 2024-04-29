using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class AddVoitureColonneModele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModeleId",
                table: "Voiture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Voiture_ModeleId",
                table: "Voiture",
                column: "ModeleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Voiture_Modele_ModeleId",
                table: "Voiture",
                column: "ModeleId",
                principalTable: "Modele",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voiture_Modele_ModeleId",
                table: "Voiture");

            migrationBuilder.DropIndex(
                name: "IX_Voiture_ModeleId",
                table: "Voiture");

            migrationBuilder.DropColumn(
                name: "ModeleId",
                table: "Voiture");
        }
    }
}
