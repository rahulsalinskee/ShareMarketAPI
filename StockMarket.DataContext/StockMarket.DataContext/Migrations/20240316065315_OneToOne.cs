using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockMarket.DataContext.Migrations
{
    /// <inheritdoc />
    public partial class OneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "93f901ca-f70f-423b-b8c0-ea107ec151a2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3c2f8d3-12d9-4f31-982c-76f4ca858e1f");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TblComments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserModelId",
                table: "TblComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "04a361a5-968d-4173-b4cb-bfff6fedc9a4", null, "Admin", "ADMIN" },
                    { "d98b460a-c428-45e1-9643-0a40ff36c16b", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TblComments_UserModelId",
                table: "TblComments",
                column: "UserModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TblComments_AspNetUsers_UserModelId",
                table: "TblComments",
                column: "UserModelId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TblComments_AspNetUsers_UserModelId",
                table: "TblComments");

            migrationBuilder.DropIndex(
                name: "IX_TblComments_UserModelId",
                table: "TblComments");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04a361a5-968d-4173-b4cb-bfff6fedc9a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d98b460a-c428-45e1-9643-0a40ff36c16b");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TblComments");

            migrationBuilder.DropColumn(
                name: "UserModelId",
                table: "TblComments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "93f901ca-f70f-423b-b8c0-ea107ec151a2", null, "Admin", "ADMIN" },
                    { "a3c2f8d3-12d9-4f31-982c-76f4ca858e1f", null, "User", "USER" }
                });
        }
    }
}
