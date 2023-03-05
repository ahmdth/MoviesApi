using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class AddGeresTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Action description", "Action" },
                    { 2, "Drama description", "Drama" },
                    { 3, "Crime description", "Crime" },
                    { 4, "Biography description", "Biography" },
                    { 5, "Adventure description", "Adventure" },
                    { 6, "Hoirrorion description", "Hoirror" },
                    { 7, "ActRomanceion description", "Romance" },
                    { 8, "Commedy description", "Commedy" },
                    { 9, "War description", "War" },
                    { 10, "Fantasy description", "Fantasy" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
