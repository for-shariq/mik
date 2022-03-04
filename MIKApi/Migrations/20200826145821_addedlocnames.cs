using Microsoft.EntityFrameworkCore.Migrations;

namespace MIKApi.Migrations
{
    public partial class addedlocnames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubLocationName",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentName",
                table: "LocationGroups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubLocationName",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ParentName",
                table: "LocationGroups");
        }
    }
}
