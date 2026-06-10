using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebScrappinDemo.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBiografiaYNuevosgrupos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sexenios",
                table: "Docentes",
                newName: "SexeniosTransferencia");

            migrationBuilder.RenameColumn(
                name: "Biografia",
                table: "Docentes",
                newName: "GrupoInvestigación");

            migrationBuilder.AddColumn<string>(
                name: "CentroInvestigación",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrupoDocente",
                table: "Docentes",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SexeniosInvestigación",
                table: "Docentes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TieneBiografia",
                table: "Docentes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CentroInvestigación",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "GrupoDocente",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "SexeniosInvestigación",
                table: "Docentes");

            migrationBuilder.DropColumn(
                name: "TieneBiografia",
                table: "Docentes");

            migrationBuilder.RenameColumn(
                name: "SexeniosTransferencia",
                table: "Docentes",
                newName: "Sexenios");

            migrationBuilder.RenameColumn(
                name: "GrupoInvestigación",
                table: "Docentes",
                newName: "Biografia");
        }
    }
}
