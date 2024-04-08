using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class NullableDateVente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoitureVente_VoitureId",
                table: "VoitureVente");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateVente",
                table: "VoitureVente",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_VoitureVente_VoitureId",
                table: "VoitureVente",
                column: "VoitureId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VoitureVente_VoitureId",
                table: "VoitureVente");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateVente",
                table: "VoitureVente",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VoitureVente_VoitureId",
                table: "VoitureVente",
                column: "VoitureId");
        }
    }
}
