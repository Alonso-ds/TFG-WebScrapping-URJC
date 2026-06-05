using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScrappinDemo.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosScrapingProfundo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Docentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    UrlPerfil = table.Column<string>(type: "TEXT", nullable: false),
                    Cargo = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Centro = table.Column<string>(type: "TEXT", nullable: true),
                    Departamento = table.Column<string>(type: "TEXT", nullable: true),
                    Area = table.Column<string>(type: "TEXT", nullable: true),
                    Biografia = table.Column<string>(type: "TEXT", nullable: true),
                    Quinquenios = table.Column<int>(type: "INTEGER", nullable: true),
                    Sexenios = table.Column<int>(type: "INTEGER", nullable: true),
                    Docentia = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docentes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Docentes");
        }
    }
}
