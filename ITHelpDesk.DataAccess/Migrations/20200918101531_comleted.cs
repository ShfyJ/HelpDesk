using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITHelpDesk.DataAccess.Migrations
{
    public partial class comleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RDateTime",
                table: "Request",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Completed_at",
                table: "Request",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed_at",
                table: "Request");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RDateTime",
                table: "Request",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
