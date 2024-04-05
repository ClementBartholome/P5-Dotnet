using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class AddVoitureFinitionReparationsCout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CoutReparations",
                table: "Voiture",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "FinitionId",
                table: "Voiture",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Reparations",
                table: "Voiture",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Voiture_FinitionId",
                table: "Voiture",
                column: "FinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Voiture_Finition_FinitionId",
                table: "Voiture",
                column: "FinitionId",
                principalTable: "Finition",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voiture_Finition_FinitionId",
                table: "Voiture");

            migrationBuilder.DropIndex(
                name: "IX_Voiture_FinitionId",
                table: "Voiture");

            migrationBuilder.DropColumn(
                name: "CoutReparations",
                table: "Voiture");

            migrationBuilder.DropColumn(
                name: "FinitionId",
                table: "Voiture");

            migrationBuilder.DropColumn(
                name: "Reparations",
                table: "Voiture");
        }
    }
}
