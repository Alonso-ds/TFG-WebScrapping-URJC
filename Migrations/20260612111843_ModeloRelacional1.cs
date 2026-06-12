using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScrappinDemo.Migrations
{
    /// <inheritdoc />
    public partial class ModeloRelacional1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", nullable: false),
                    FechaInicio = table.Column<string>(type: "TEXT", nullable: true),
                    FechaFinal = table.Column<string>(type: "TEXT", nullable: true),
                    EntidadFinanciadora = table.Column<string>(type: "TEXT", nullable: true),
                    RefExterna = table.Column<string>(type: "TEXT", nullable: true),
                    RefInterna = table.Column<string>(type: "TEXT", nullable: true),
                    InvPrincipales = table.Column<string>(type: "TEXT", nullable: true),
                    Investigadores = table.Column<string>(type: "TEXT", nullable: true),
                    InvestigadoresTecnicos = table.Column<string>(type: "TEXT", nullable: true),
                    Colaboradores = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocenteProyecto",
                columns: table => new
                {
                    DocentesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProyectosId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocenteProyecto", x => new { x.DocentesId, x.ProyectosId });
                    table.ForeignKey(
                        name: "FK_DocenteProyecto_Docentes_DocentesId",
                        column: x => x.DocentesId,
                        principalTable: "Docentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocenteProyecto_Proyectos_ProyectosId",
                        column: x => x.ProyectosId,
                        principalTable: "Proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocenteProyecto_ProyectosId",
                table: "DocenteProyecto",
                column: "ProyectosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocenteProyecto");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
