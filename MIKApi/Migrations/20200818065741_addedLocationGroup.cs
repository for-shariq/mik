using Microsoft.EntityFrameworkCore.Migrations;

namespace MIKApi.Migrations
{
    public partial class addedLocationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationGroupId",
                table: "Locations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocationGroupId",
                table: "Locations");
        }
    }
}
