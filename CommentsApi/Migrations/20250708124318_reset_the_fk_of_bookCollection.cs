using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentsApi.Migrations
{
    /// <inheritdoc />
    public partial class reset_the_fk_of_bookCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "BookCollects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookCollects_BookInfoId",
                table: "BookCollects",
                column: "BookInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCollects_Books_BookInfoId",
                table: "BookCollects",
                column: "BookInfoId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollects_Books_BookInfoId",
                table: "BookCollects");

            migrationBuilder.DropIndex(
                name: "IX_BookCollects_BookInfoId",
                table: "BookCollects");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "BookCollects");
        }
    }
}
