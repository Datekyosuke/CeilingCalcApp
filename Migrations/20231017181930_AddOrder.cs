using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiDB.Migrations
{
    /// <inheritdoc />
    public partial class AddOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dealers_ID",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Orders",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ID",
                table: "Orders",
                newName: "IX_Orders_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dealers_Id",
                table: "Orders",
                column: "Id",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dealers_Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Orders",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                newName: "IX_Orders_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dealers_ID",
                table: "Orders",
                column: "ID",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
