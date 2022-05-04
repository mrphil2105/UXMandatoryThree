using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeAnalog.Migrations
{
    public partial class AddUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTickets_AspNetUsers_UserId",
                table: "InventoryTickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InventoryTickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTickets_AspNetUsers_UserId",
                table: "InventoryTickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTickets_AspNetUsers_UserId",
                table: "InventoryTickets");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InventoryTickets",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTickets_AspNetUsers_UserId",
                table: "InventoryTickets",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
