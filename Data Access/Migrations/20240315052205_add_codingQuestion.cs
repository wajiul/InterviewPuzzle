using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPuzzle.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcodingQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "codingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_codingQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "solutions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodingQuestionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_solutions_codingQuestions_CodingQuestionId",
                        column: x => x.CodingQuestionId,
                        principalTable: "codingQuestions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_solutions_CodingQuestionId",
                table: "solutions",
                column: "CodingQuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "solutions");

            migrationBuilder.DropTable(
                name: "codingQuestions");
        }
    }
}
