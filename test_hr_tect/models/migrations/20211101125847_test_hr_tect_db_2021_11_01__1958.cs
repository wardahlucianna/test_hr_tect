using Microsoft.EntityFrameworkCore.Migrations;

namespace test_hr_tect.models.migrations
{
    public partial class test_hr_tect_db_2021_11_01__1958 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "m_employe_password",
                table: "m_employes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "m_employe_password",
                table: "m_employes");
        }
    }
}
