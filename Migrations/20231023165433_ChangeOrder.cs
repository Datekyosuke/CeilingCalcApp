using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiDB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Dealers_Id",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Dealers",
                newName: "DealerId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "DealerId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DealerId",
                table: "Orders",
                column: "DealerId");

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

            migrationBuilder.DropIndex(
                name: "IX_Orders_DealerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DealerId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DealerId",
                table: "Dealers",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Dealers_Id",
                table: "Orders",
                column: "Id",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
