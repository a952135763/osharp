using Microsoft.EntityFrameworkCore.Migrations;

namespace KaPai.Pay.Web.Migrations
{
    public partial class init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FreezeAmount",
                table: "Amounts",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreezeAmount",
                table: "Amounts");
        }
    }
}
