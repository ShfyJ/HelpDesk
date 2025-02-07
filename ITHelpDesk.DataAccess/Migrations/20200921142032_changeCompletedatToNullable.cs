using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ITHelpDesk.DataAccess.Migrations
{
    public partial class changeCompletedatToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Room",
                table: "Request");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed_at",
                table: "Request",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Completed_at",
                table: "Request",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
