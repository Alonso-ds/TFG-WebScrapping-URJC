using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScrappinDemo.Migrations
{
    /// <inheritdoc />
    public partial class AsignaturasYTfe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Asignaturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Codigo = table.Column<string>(type: "TEXT", nullable: true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: true),
                    TipoEstudio = table.Column<string>(type: "TEXT", nullable: true),
                    Grado = table.Column<string>(type: "TEXT", nullable: true),
                    Horas = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tfes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<string>(type: "TEXT", nullable: true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: true),
                    FechaDefensa = table.Column<string>(type: "TEXT", nullable: true),
                    Grado = table.Column<string>(type: "TEXT", nullable: true),
                    Tutor = table.Column<string>(type: "TEXT", nullable: true),
                    Cotutor = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tfes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsignaturaDocente",
                columns: table => new
                {
                    AsignaturasId = table.Column<int>(type: "INTEGER", nullable: false),
                    DocentesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignaturaDocente", x => new { x.AsignaturasId, x.DocentesId });
                    table.ForeignKey(
                        name: "FK_AsignaturaDocente_Asignaturas_AsignaturasId",
                        column: x => x.AsignaturasId,
                        principalTable: "Asignaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignaturaDocente_Docentes_DocentesId",
                        column: x => x.DocentesId,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocenteTfe",
                columns: table => new
                {
                    DocentesId = table.Column<int>(type: "INTEGER", nullable: false),
                    TfesId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteTfe", x => new { x.DocentesId, x.TfesId });
                    table.ForeignKey(
                        name: "FK_DocenteTfe_Docentes_DocentesId",
                        column: x => x.DocentesId,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteTfe_Tfes_TfesId",
                        column: x => x.TfesId,
                        principalTable: "Tfes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignaturaDocente_DocentesId",
                table: "AsignaturaDocente",
                column: "DocentesId");

            migrationBuilder.CreateIndex(
                name: "IX_DocenteTfe_TfesId",
                table: "DocenteTfe",
                column: "TfesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsignaturaDocente");

            migrationBuilder.DropTable(
                name: "DocenteTfe");

            migrationBuilder.DropTable(
                name: "Asignaturas");

            migrationBuilder.DropTable(
                name: "Tfes");
        }
    }
}
