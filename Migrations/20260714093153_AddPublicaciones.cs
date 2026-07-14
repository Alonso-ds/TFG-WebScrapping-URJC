using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScrappinDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Publicaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    Autores = table.Column<string>(type: "TEXT", nullable: true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Fecha = table.Column<string>(type: "TEXT", nullable: true),
                    Doi = table.Column<string>(type: "TEXT", nullable: true),
                    Cuartil = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocentePublicacion",
                columns: table => new
                {
                    DocentesId = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicacionesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocentePublicacion", x => new { x.DocentesId, x.PublicacionesId });
                    table.ForeignKey(
                        name: "FK_DocentePublicacion_Docentes_DocentesId",
                        column: x => x.DocentesId,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocentePublicacion_Publicaciones_PublicacionesId",
                        column: x => x.PublicacionesId,
                        principalTable: "Publicaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocentePublicacion_PublicacionesId",
                table: "DocentePublicacion",
                column: "PublicacionesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocentePublicacion");

            migrationBuilder.DropTable(
                name: "Publicaciones");
        }
    }
}
