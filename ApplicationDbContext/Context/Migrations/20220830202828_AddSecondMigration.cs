using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationDbContext.Context.Migrations
{
    public partial class AddSecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OpportunityList",
                columns: table => new
                {
                    OpportunityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Office = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    EmploymentId = table.Column<int>(nullable: false),
                    Experience = table.Column<int>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Number = table.Column<string>(nullable: true),
                    Complement = table.Column<string>(nullable: true),
                    Neighborhood = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CandidateId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityList", x => x.OpportunityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpportunityList");
        }
    }
}
