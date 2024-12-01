using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugioVerde.Migrations
{
    /// <inheritdoc />
    public partial class AddEncuestaSatisfaccion1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpleadoId",
                table: "EncuestasSatisfaccion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HabitacionId",
                table: "EncuestasSatisfaccion",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EncuestasSatisfaccion_EmpleadoId",
                table: "EncuestasSatisfaccion",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EncuestasSatisfaccion_HabitacionId",
                table: "EncuestasSatisfaccion",
                column: "HabitacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_EncuestaSatisfaccion_Empleado",
                table: "EncuestasSatisfaccion",
                column: "EmpleadoId",
                principalTable: "Empleado",
                principalColumn: "EmpleadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EncuestaSatisfaccion_Habitacion",
                table: "EncuestasSatisfaccion",
                column: "HabitacionId",
                principalTable: "Habitacion",
                principalColumn: "HabitacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncuestaSatisfaccion_Empleado",
                table: "EncuestasSatisfaccion");

            migrationBuilder.DropForeignKey(
                name: "FK_EncuestaSatisfaccion_Habitacion",
                table: "EncuestasSatisfaccion");

            migrationBuilder.DropIndex(
                name: "IX_EncuestasSatisfaccion_EmpleadoId",
                table: "EncuestasSatisfaccion");

            migrationBuilder.DropIndex(
                name: "IX_EncuestasSatisfaccion_HabitacionId",
                table: "EncuestasSatisfaccion");

            migrationBuilder.DropColumn(
                name: "EmpleadoId",
                table: "EncuestasSatisfaccion");

            migrationBuilder.DropColumn(
                name: "HabitacionId",
                table: "EncuestasSatisfaccion");
        }
    }
}
