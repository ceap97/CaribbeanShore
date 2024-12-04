using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugioVerde.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Imagen",
                table: "Servicio",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pago_MetodoDePagoId",
                table: "Pago",
                column: "MetodoDePagoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pago_MetodoDePago_MetodoDePagoId",
                table: "Pago",
                column: "MetodoDePagoId",
                principalTable: "MetodoDePago",
                principalColumn: "MetodoDePagoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pago_MetodoDePago_MetodoDePagoId",
                table: "Pago");

            migrationBuilder.DropIndex(
                name: "IX_Pago_MetodoDePagoId",
                table: "Pago");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Imagen",
                table: "Servicio",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}


