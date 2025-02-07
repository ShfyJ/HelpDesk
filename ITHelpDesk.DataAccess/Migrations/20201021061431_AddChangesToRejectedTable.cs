using Microsoft.EntityFrameworkCore.Migrations;

namespace ITHelpDesk.DataAccess.Migrations
{
    public partial class AddChangesToRejectedTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Request",
                table: "Rejecteds");

            migrationBuilder.DropColumn(
                name: "Worker",
                table: "Rejecteds");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Rejecteds",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkerId",
                table: "Rejecteds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rejecteds_RequestId",
                table: "Rejecteds",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Rejecteds_WorkerId",
                table: "Rejecteds",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rejecteds_Request_RequestId",
                table: "Rejecteds",
                column: "RequestId",
                principalTable: "Request",
                principalColumn: "RequestId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rejecteds_Workers_WorkerId",
                table: "Rejecteds",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "WorkerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rejecteds_Request_RequestId",
                table: "Rejecteds");

            migrationBuilder.DropForeignKey(
                name: "FK_Rejecteds_Workers_WorkerId",
                table: "Rejecteds");

            migrationBuilder.DropIndex(
                name: "IX_Rejecteds_RequestId",
                table: "Rejecteds");

            migrationBuilder.DropIndex(
                name: "IX_Rejecteds_WorkerId",
                table: "Rejecteds");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Rejecteds");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Rejecteds");

            migrationBuilder.AddColumn<int>(
                name: "Request",
                table: "Rejecteds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Worker",
                table: "Rejecteds",
                type: "int",
                nullable: true);
        }
    }
}
