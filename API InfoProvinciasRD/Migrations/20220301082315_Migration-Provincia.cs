using Microsoft.EntityFrameworkCore.Migrations;

namespace API_InfoProvinciasRD.Migrations
{
    public partial class MigrationProvincia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provincia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RutaImagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fundacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Superficie = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Provincia_Region_regionId",
                        column: x => x.regionId,
                        principalTable: "Region",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Provincia_regionId",
                table: "Provincia",
                column: "regionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Provincia");
        }
    }
}
