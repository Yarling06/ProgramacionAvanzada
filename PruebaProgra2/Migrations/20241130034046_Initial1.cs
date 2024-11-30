using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaProgra2.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosTarea",
                columns: table => new
                {
                    EstadoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EstadosT__FEF86B605F61D9F6", x => x.EstadoID);
                });

            migrationBuilder.CreateTable(
                name: "PrioridadesTarea",
                columns: table => new
                {
                    PrioridadID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NivelPrioridad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Priorida__393917CE6F4E24DE", x => x.PrioridadID);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    TareaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrioridadID = table.Column<int>(type: "int", nullable: false),
                    EstadoID = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    FechaEjecucion = table.Column<DateTime>(type: "datetime", nullable: true),
                    FechaFinalizacion = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tareas__5CD836717B6C9285", x => x.TareaID);
                    table.ForeignKey(
                        name: "FK_Tareas_EstadosTarea",
                        column: x => x.EstadoID,
                        principalTable: "EstadosTarea",
                        principalColumn: "EstadoID");
                    table.ForeignKey(
                        name: "FK_Tareas_PrioridadesTarea",
                        column: x => x.PrioridadID,
                        principalTable: "PrioridadesTarea",
                        principalColumn: "PrioridadID");
                });

            migrationBuilder.CreateTable(
                name: "LogsEjecucion",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TareaID = table.Column<int>(type: "int", nullable: false),
                    EstadoID = table.Column<int>(type: "int", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaLog = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LogsEjec__5E5499A88F583209", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_LogsEjecucion_EstadosTarea",
                        column: x => x.EstadoID,
                        principalTable: "EstadosTarea",
                        principalColumn: "EstadoID");
                    table.ForeignKey(
                        name: "FK_LogsEjecucion_Tareas",
                        column: x => x.TareaID,
                        principalTable: "Tareas",
                        principalColumn: "TareaID");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__EstadosT__6CE50615FBEA5A6A",
                table: "EstadosTarea",
                column: "NombreEstado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogsEjecucion_EstadoID",
                table: "LogsEjecucion",
                column: "EstadoID");

            migrationBuilder.CreateIndex(
                name: "IX_LogsEjecucion_TareaID",
                table: "LogsEjecucion",
                column: "TareaID");

            migrationBuilder.CreateIndex(
                name: "UQ__Priorida__9A68DDA1AEAA00E4",
                table: "PrioridadesTarea",
                column: "NivelPrioridad",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_EstadoID",
                table: "Tareas",
                column: "EstadoID");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_PrioridadID",
                table: "Tareas",
                column: "PrioridadID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsEjecucion");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "EstadosTarea");

            migrationBuilder.DropTable(
                name: "PrioridadesTarea");
        }
    }
}
