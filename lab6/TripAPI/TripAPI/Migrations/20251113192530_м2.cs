using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripAPI.Migrations
{
    /// <inheritdoc />
    public partial class м2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLiners_Routes_RouteId",
                table: "RouteLiners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Routes",
                table: "Routes");

            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "CruiseRoutes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CruiseRoutes",
                table: "CruiseRoutes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLiners_CruiseRoutes_RouteId",
                table: "RouteLiners",
                column: "RouteId",
                principalTable: "CruiseRoutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteLiners_CruiseRoutes_RouteId",
                table: "RouteLiners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CruiseRoutes",
                table: "CruiseRoutes");

            migrationBuilder.RenameTable(
                name: "CruiseRoutes",
                newName: "Routes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Routes",
                table: "Routes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLiners_Routes_RouteId",
                table: "RouteLiners",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
