using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApplicationDbContext.Context.Migrations
{
    public partial class AddFourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "CandidateId",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "City",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Complement",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "State",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "OpportunityList");

            migrationBuilder.AddColumn<DateTime>(
                name: "BlockDate",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BlockReason",
                table: "Person",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "OpportunityList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentName",
                table: "OpportunityList",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Local",
                table: "OpportunityList",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "OpportunityList",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "OpportunityList",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StatusName",
                table: "OpportunityList",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Opportunity",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<double>(
                name: "Salary",
                table: "Opportunity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Opportunity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OpportunityCriterion",
                columns: table => new
                {
                    OpportunityCriterionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpportunityId = table.Column<int>(nullable: false),
                    Criterion = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Weight = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpportunityCriterion", x => x.OpportunityCriterionId);
                });

            migrationBuilder.CreateTable(
                name: "Response",
                columns: table => new
                {
                    ResponseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OpportunityId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CandidateId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    NotesByOpportunity = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Response", x => x.ResponseId);
                });

            migrationBuilder.CreateTable(
                name: "ResponseCriterion",
                columns: table => new
                {
                    ResponseCriterionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(nullable: false),
                    OpportunityCriterionId = table.Column<int>(nullable: false),
                    AnswerCriterion = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseCriterion", x => x.ResponseCriterionId);
                });

            migrationBuilder.CreateTable(
                name: "UserList",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Cnpj = table.Column<string>(nullable: true),
                    Cpf = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    MobileNumber = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserList", x => x.UserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpportunityCriterion");

            migrationBuilder.DropTable(
                name: "Response");

            migrationBuilder.DropTable(
                name: "ResponseCriterion");

            migrationBuilder.DropTable(
                name: "UserList");

            migrationBuilder.DropColumn(
                name: "BlockDate",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "BlockReason",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "EmploymentName",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Local",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "StatusName",
                table: "OpportunityList");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Opportunity");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Opportunity");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CandidateId",
                table: "OpportunityList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complement",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "OpportunityList",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Opportunity",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
