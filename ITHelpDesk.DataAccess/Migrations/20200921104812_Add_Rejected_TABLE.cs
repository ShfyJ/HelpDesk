using Microsoft.EntityFrameworkCore.Migrations;

namespace ITHelpDesk.DataAccess.Migrations
{
    public partial class Add_Rejected_TABLE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rejecteds",
                columns: table => new
                {
                    RejectedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Request = table.Column<int>(nullable: false),
                    Worker = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rejecteds", x => x.RejectedId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rejecteds");
        }
    }
}
