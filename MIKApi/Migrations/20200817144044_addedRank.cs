using Microsoft.EntityFrameworkCore.Migrations;

namespace MIKApi.Migrations
{
    public partial class addedRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Rank",
                table: "Nauhas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Nauhas");
        }
    }
}
