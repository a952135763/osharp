using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace KaPai.Pay.Web.Migrations
{
    public partial class init9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalRegion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Pid = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Postcode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalRegion", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "KeyValue",
                keyColumn: "Id",
                keyValue: new Guid("534d7813-0eea-44cc-b88e-a9cb010c6981"),
                column: "ValueJson",
                value: "\".Net Core\"");

            migrationBuilder.UpdateData(
                table: "KeyValue",
                keyColumn: "Id",
                keyValue: new Guid("977e4bba-97b2-4759-a768-a9cb010c698c"),
                column: "ValueJson",
                value: "\" .NetStandard2.0 & Angular7\"");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalRegion_Level",
                table: "GlobalRegion",
                column: "Level");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalRegion_Pid",
                table: "GlobalRegion",
                column: "Pid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalRegion");

            migrationBuilder.UpdateData(
                table: "KeyValue",
                keyColumn: "Id",
                keyValue: new Guid("534d7813-0eea-44cc-b88e-a9cb010c6981"),
                column: "ValueJson",
                value: "\"OSHARP\"");

            migrationBuilder.UpdateData(
                table: "KeyValue",
                keyColumn: "Id",
                keyValue: new Guid("977e4bba-97b2-4759-a768-a9cb010c698c"),
                column: "ValueJson",
                value: "\"Osharp with .NetStandard2.0 & Angular6\"");
        }
    }
}
