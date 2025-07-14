using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommentsApi.Migrations
{
    /// <inheritdoc />
    public partial class add_navigation_to_bookinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookInfoId",
                table: "BookLike",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookLike_BookInfoId",
                table: "BookLike",
                column: "BookInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLike_Books_BookInfoId",
                table: "BookLike",
                column: "BookInfoId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLike_Books_BookInfoId",
                table: "BookLike");

            migrationBuilder.DropIndex(
                name: "IX_BookLike_BookInfoId",
                table: "BookLike");

            migrationBuilder.DropColumn(
                name: "BookInfoId",
                table: "BookLike");
        }
    }
}
