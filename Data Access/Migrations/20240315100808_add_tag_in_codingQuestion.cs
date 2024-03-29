using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPuzzle.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addtagincodingQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "codingQuestions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "codingQuestions");
        }
    }
}
