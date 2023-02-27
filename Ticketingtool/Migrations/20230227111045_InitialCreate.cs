using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ticketingtool.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JiraTaskDetails",
                columns: table => new
                {
                    issuetype = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jiraProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reporter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    jiraProjectKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LOB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    issueKey = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JiraTaskDetails");
        }
    }
}
