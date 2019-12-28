using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KaPai.Pay.Web.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedTime",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastUpdaterId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderBackLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(maxLength: 500, nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    OrderId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBackLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBackLog_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderBackLog_OrderId",
                table: "OrderBackLog",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderBackLog");

            migrationBuilder.DropColumn(
                name: "LastUpdatedTime",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastUpdaterId",
                table: "Orders");
        }
    }
}
