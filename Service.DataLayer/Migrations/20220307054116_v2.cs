using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.DataLayer.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    UserPasswordLength = table.Column<int>(type: "int", nullable: false),
                    UserSessionTimeOut = table.Column<int>(type: "int", nullable: false),
                    MaxLimitToLockUser = table.Column<int>(type: "int", nullable: false),
                    UserConfirmPolicyTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserConfirmPolicy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmpDataApiUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Setting");
        }
    }
}
