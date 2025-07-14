using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentsApi.Migrations
{
    /// <inheritdoc />
    public partial class fix_relationship_of_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCollects_Books_BookInfoId",
                table: "BookCollects");

            migrationBuilder.DropForeignKey(
                name: "FK_BookLike_Books_BookInfoId",
                table: "BookLike");

            migrationBuilder.DropIndex(
                name: "IX_BookLike_BookInfoId",
                table: "BookLike");

            migrationBuilder.DropIndex(
                name: "IX_BookCollects_BookInfoId",
                table: "BookCollects");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "BookLike");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "BookCollects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "BookLike",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "BookCollects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookLike_BookInfoId",
                table: "BookLike",
                column: "BookInfoId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_BookLike_Books_BookInfoId",
                table: "BookLike",
                column: "BookInfoId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
