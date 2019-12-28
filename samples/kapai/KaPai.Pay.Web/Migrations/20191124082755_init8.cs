using Microsoft.EntityFrameworkCore.Migrations;

namespace KaPai.Pay.Web.Migrations
{
    public partial class init8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FreezePoint",
                table: "Points",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreezePoint",
                table: "Points");
        }
    }
}
