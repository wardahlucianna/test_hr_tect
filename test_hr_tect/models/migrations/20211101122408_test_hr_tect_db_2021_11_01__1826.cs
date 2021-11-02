using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace test_hr_tect.models.migrations
{
    public partial class test_hr_tect_db_2021_11_01__1826 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "m_companies",
                columns: table => new
                {
                    m_company_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    m_company_name = table.Column<string>(nullable: false),
                    m_company_email = table.Column<string>(nullable: false),
                    m_company_website = table.Column<string>(nullable: false),
                    m_company_logo = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_companies", x => x.m_company_id);
                });

            migrationBuilder.CreateTable(
                name: "m_feature_group",
                columns: table => new
                {
                    m_feature_group_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    m_application_id = table.Column<int>(nullable: false),
                    feature_group_name = table.Column<string>(maxLength: 100, nullable: false),
                    feature_group_url = table.Column<string>(maxLength: 100, nullable: false),
                    feature_group_status = table.Column<string>(maxLength: 100, nullable: false),
                    feature_group_squance = table.Column<int>(maxLength: 100, nullable: false),
                    feature_group_create_at = table.Column<DateTime>(maxLength: 10, nullable: true),
                    feature_group_create_by = table.Column<int>(maxLength: 10, nullable: true),
                    feature_group_update_at = table.Column<DateTime>(maxLength: 10, nullable: true),
                    feature_group_update_by = table.Column<int>(maxLength: 10, nullable: true),
                    feature_group_icon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_feature_group", x => x.m_feature_group_id);
                });

            migrationBuilder.CreateTable(
                name: "m_parameters",
                columns: table => new
                {
                    m_parameter_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    parameter_group = table.Column<string>(nullable: false),
                    parameter_key = table.Column<string>(nullable: false),
                    parameter_value = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_parameters", x => x.m_parameter_id);
                });

            migrationBuilder.CreateTable(
                name: "m_employes",
                columns: table => new
                {
                    m_employe_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    m_employe_first_name = table.Column<string>(nullable: false),
                    m_employe_last_name = table.Column<string>(nullable: false),
                    m_employe_email = table.Column<string>(nullable: false),
                    m_employe_phone = table.Column<string>(nullable: false),
                    m_company_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_employes", x => x.m_employe_id);
                    table.ForeignKey(
                        name: "FK_m_employes_m_companies_m_company_id",
                        column: x => x.m_company_id,
                        principalTable: "m_companies",
                        principalColumn: "m_company_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "m_feature",
                columns: table => new
                {
                    m_feature_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    m_feature_group_id = table.Column<int>(nullable: false),
                    feature_name = table.Column<string>(maxLength: 100, nullable: false),
                    feature_sequence = table.Column<int>(nullable: false),
                    feature_url = table.Column<string>(maxLength: 255, nullable: false),
                    feature_icon = table.Column<string>(maxLength: 50, nullable: false),
                    feature_visible = table.Column<bool>(nullable: false),
                    feature_status = table.Column<string>(nullable: false),
                    feature_create_at = table.Column<DateTime>(maxLength: 10, nullable: true),
                    feature_create_by = table.Column<int>(maxLength: 10, nullable: true),
                    feature_update_at = table.Column<DateTime>(maxLength: 10, nullable: true),
                    feature_update_by = table.Column<int>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_m_feature", x => x.m_feature_id);
                    table.ForeignKey(
                        name: "FK_m_feature_m_feature_group_m_feature_group_id",
                        column: x => x.m_feature_group_id,
                        principalTable: "m_feature_group",
                        principalColumn: "m_feature_group_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_m_employes_m_company_id",
                table: "m_employes",
                column: "m_company_id");

            migrationBuilder.CreateIndex(
                name: "IX_m_feature_m_feature_group_id",
                table: "m_feature",
                column: "m_feature_group_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "m_employes");

            migrationBuilder.DropTable(
                name: "m_feature");

            migrationBuilder.DropTable(
                name: "m_parameters");

            migrationBuilder.DropTable(
                name: "m_companies");

            migrationBuilder.DropTable(
                name: "m_feature_group");
        }
    }
}
