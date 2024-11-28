using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RefugioVerde.Migrations
{
    /// <inheritdoc />
    public partial class CambiarComprobanteANvarcharMax : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comprobante",
                table: "Pago",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Comprobante",
                table: "Pago",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

    }
}
