using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RotaViagem.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rotas",
                columns: table => new
                {
                    Origem = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Destino = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Custo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rotas", x => new { x.Origem, x.Destino });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rotas");
        }
    }
}
