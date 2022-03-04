using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIKApi.Migrations
{
    public partial class addedvideo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(500)", nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EmbedUrl = table.Column<string>(nullable: true),
                    ChannelName = table.Column<string>(nullable: true),
                    ContentType = table.Column<int>(nullable: false),
                    Approved = table.Column<bool>(nullable: false),
                    Tags = table.Column<string>(nullable: true),
                    Rank = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Videos");
        }
    }
}
