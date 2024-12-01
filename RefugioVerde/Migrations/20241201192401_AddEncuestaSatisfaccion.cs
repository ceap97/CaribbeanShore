using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugioVerde.Migrations
{
    /// <inheritdoc />
    public partial class AddEncuestaSatisfaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comprobante",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "EncuestasSatisfaccion",
                columns: table => new
                {
                    EncuestaSatisfaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalificacionGeneral = table.Column<int>(type: "int", nullable: false),
                    CalificacionHabitacion = table.Column<int>(type: "int", nullable: false),
                    CalificacionAtencion = table.Column<int>(type: "int", nullable: false),
                    Comentarios = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncuestasSatisfaccion", x => x.EncuestaSatisfaccionId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncuestasSatisfaccion");

            migrationBuilder.AlterColumn<string>(
                name: "Comprobante",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
