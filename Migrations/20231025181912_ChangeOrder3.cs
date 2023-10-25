using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiDB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrder3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dealers_DealerInfoId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DealerInfoId",
                table: "Orders",
                newName: "DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DealerInfoId",
                table: "Orders",
                newName: "IX_Orders_DealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dealers_DealerId",
                table: "Orders",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "DealerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dealers_DealerId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DealerId",
                table: "Orders",
                newName: "DealerInfoId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DealerId",
                table: "Orders",
                newName: "IX_Orders_DealerInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dealers_DealerInfoId",
                table: "Orders",
                column: "DealerInfoId",
                principalTable: "Dealers",
                principalColumn: "DealerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
