using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KaPai.Pay.Web.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "CreatedTime", "DeletedTime", "IsAdmin", "IsDefault", "IsLocked", "IsSystem", "MessageId", "Name", "NormalizedName", "Remark" },
                values: new object[,]
                {
                    { 2, "0147D7FA-BDA8-4319-BC54-EBD59B6BD8F6", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, true, null, "供应商", "供应商", "业务供应商" },
                    { 3, "14B6B0D4-D9B9-41B6-8018-4E0EE4A16F94", new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, false, false, true, null, "商户", "商户", "业务商户" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
