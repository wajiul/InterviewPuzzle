using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewPuzzle.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addOptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Option_mcqs_MCQId",
                table: "Option");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Option",
                table: "Option");

            migrationBuilder.RenameTable(
                name: "Option",
                newName: "options");

            migrationBuilder.RenameIndex(
                name: "IX_Option_MCQId",
                table: "options",
                newName: "IX_options_MCQId");

            migrationBuilder.AlterColumn<int>(
                name: "MCQId",
                table: "options",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_options",
                table: "options",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_options_mcqs_MCQId",
                table: "options",
                column: "MCQId",
                principalTable: "mcqs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_options_mcqs_MCQId",
                table: "options");

            migrationBuilder.DropPrimaryKey(
                name: "PK_options",
                table: "options");

            migrationBuilder.RenameTable(
                name: "options",
                newName: "Option");

            migrationBuilder.RenameIndex(
                name: "IX_options_MCQId",
                table: "Option",
                newName: "IX_Option_MCQId");

            migrationBuilder.AlterColumn<int>(
                name: "MCQId",
                table: "Option",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Option",
                table: "Option",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Option_mcqs_MCQId",
                table: "Option",
                column: "MCQId",
                principalTable: "mcqs",
                principalColumn: "Id");
        }
    }
}
