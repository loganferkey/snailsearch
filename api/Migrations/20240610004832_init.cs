using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Relics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Rank = table.Column<string>(type: "TEXT", nullable: false),
                    AFFCT = table.Column<string>(type: "TEXT", nullable: false),
                    Source = table.Column<string>(type: "TEXT", nullable: false),
                    FAME = table.Column<int>(type: "INTEGER", nullable: false),
                    ART = table.Column<int>(type: "INTEGER", nullable: false),
                    FTH = table.Column<int>(type: "INTEGER", nullable: false),
                    CIV = table.Column<int>(type: "INTEGER", nullable: false),
                    TECH = table.Column<int>(type: "INTEGER", nullable: false),
                    Special = table.Column<string>(type: "TEXT", nullable: false),
                    Skill1 = table.Column<string>(type: "TEXT", nullable: true),
                    Skill2 = table.Column<string>(type: "TEXT", nullable: true),
                    Skill3 = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Relics");
        }
    }
}
