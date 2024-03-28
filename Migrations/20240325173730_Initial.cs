using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Marque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marque", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modele",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarqueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modele", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modele_Marque_MarqueId",
                        column: x => x.MarqueId,
                        principalTable: "Marque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voiture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarqueId = table.Column<int>(type: "int", nullable: false),
                    Annee = table.Column<int>(type: "int", nullable: false),
                    DateAchat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrixAchat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voiture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voiture_Marque_MarqueId",
                        column: x => x.MarqueId,
                        principalTable: "Marque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Finition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModeleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Finition_Modele_ModeleId",
                        column: x => x.ModeleId,
                        principalTable: "Modele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Annonce",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoitureId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annonce", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Annonce_Voiture_VoitureId",
                        column: x => x.VoitureId,
                        principalTable: "Voiture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoitureVente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoitureId = table.Column<int>(type: "int", nullable: false),
                    DateDisponibilite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateVente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrixVente = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoitureVente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoitureVente_Voiture_VoitureId",
                        column: x => x.VoitureId,
                        principalTable: "Voiture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annonce_VoitureId",
                table: "Annonce",
                column: "VoitureId");

            migrationBuilder.CreateIndex(
                name: "IX_Finition_ModeleId",
                table: "Finition",
                column: "ModeleId");

            migrationBuilder.CreateIndex(
                name: "IX_Modele_MarqueId",
                table: "Modele",
                column: "MarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_Voiture_MarqueId",
                table: "Voiture",
                column: "MarqueId");

            migrationBuilder.CreateIndex(
                name: "IX_VoitureVente_VoitureId",
                table: "VoitureVente",
                column: "VoitureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annonce");

            migrationBuilder.DropTable(
                name: "Finition");

            migrationBuilder.DropTable(
                name: "VoitureVente");

            migrationBuilder.DropTable(
                name: "Modele");

            migrationBuilder.DropTable(
                name: "Voiture");

            migrationBuilder.DropTable(
                name: "Marque");
        }
    }
}
