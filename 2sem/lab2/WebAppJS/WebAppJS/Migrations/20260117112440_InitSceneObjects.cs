using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAppJS.Migrations
{
    /// <inheritdoc />
    public partial class InitSceneObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SceneObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    A = table.Column<float>(type: "real", nullable: false),
                    B = table.Column<float>(type: "real", nullable: false),
                    C = table.Column<float>(type: "real", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false),
                    Z = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SceneObjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SceneObjects");
        }
    }
}
