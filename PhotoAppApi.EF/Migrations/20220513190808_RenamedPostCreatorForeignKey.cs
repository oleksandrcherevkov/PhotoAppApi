using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhotoAppApi.EF.Migrations
{
    public partial class RenamedPostCreatorForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Posts_PostId",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComment_Users_CreatorLogin",
                table: "PostComment");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostComment",
                table: "PostComment");

            migrationBuilder.RenameTable(
                name: "PostComment",
                newName: "PostComments");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Posts",
                newName: "CreatorLogin");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CreatorId",
                table: "Posts",
                newName: "IX_Posts_CreatorLogin");

            migrationBuilder.RenameIndex(
                name: "IX_PostComment_PostId",
                table: "PostComments",
                newName: "IX_PostComments_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostComment_CreatorLogin",
                table: "PostComments",
                newName: "IX_PostComments_CreatorLogin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Users_CreatorLogin",
                table: "PostComments",
                column: "CreatorLogin",
                principalTable: "Users",
                principalColumn: "Login");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatorLogin",
                table: "Posts",
                column: "CreatorLogin",
                principalTable: "Users",
                principalColumn: "Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Users_CreatorLogin",
                table: "PostComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Users_CreatorLogin",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostComments",
                table: "PostComments");

            migrationBuilder.RenameTable(
                name: "PostComments",
                newName: "PostComment");

            migrationBuilder.RenameColumn(
                name: "CreatorLogin",
                table: "Posts",
                newName: "CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CreatorLogin",
                table: "Posts",
                newName: "IX_Posts_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_PostId",
                table: "PostComment",
                newName: "IX_PostComment_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostComments_CreatorLogin",
                table: "PostComment",
                newName: "IX_PostComment_CreatorLogin");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostComment",
                table: "PostComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Posts_PostId",
                table: "PostComment",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostComment_Users_CreatorLogin",
                table: "PostComment",
                column: "CreatorLogin",
                principalTable: "Users",
                principalColumn: "Login");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Users_CreatorId",
                table: "Posts",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Login");
        }
    }
}
