using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Express_Voitures.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "1dfb8544-4580-4963-8a7a-40f579d4d949", "admin@expressvoitures.com", "AQAAAAIAAYagAAAAEAUv4OYtrh8t6/Hdv6z+oTqE6qMquATKboRXaM7i1msjlMes9MVYGD43h4koa0hpjw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash" },
                values: new object[] { "d78806f6-efae-4101-9e5e-f4b1963a3517", "ADMIN", "AQAAAAIAAYagAAAAEIj+gVnFpqYFPSJdudF7EOULpOPbGLrbXhYQ3M23Xl9/SfsexIGAcbX9vyCUbmPFVw==" });
        }
    }
}
