using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    identityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    processorID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    administrationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    administrationOId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeePosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lawReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    serviceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    serviceURI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    dateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateDeleted = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UUID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");
        }
    }
}
