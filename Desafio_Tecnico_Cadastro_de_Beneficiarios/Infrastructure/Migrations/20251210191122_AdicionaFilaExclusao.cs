using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Tecnico_Cadastro_de_Beneficiarios.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaFilaExclusao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataSolicitacaoExclusao",
                table: "Beneficiarios",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PendenteExclusao",
                table: "Beneficiarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PrioridadeExclusao",
                table: "Beneficiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataSolicitacaoExclusao",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "PendenteExclusao",
                table: "Beneficiarios");

            migrationBuilder.DropColumn(
                name: "PrioridadeExclusao",
                table: "Beneficiarios");
        }
    }
}
