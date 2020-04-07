using Microsoft.EntityFrameworkCore.Migrations;

namespace TgAdsStatistics.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(nullable: true),
                    NumberOfAdsPosted = table.Column<int>(nullable: false),
                    OverallViews = table.Column<int>(nullable: false),
                    OverallMoneySpent = table.Column<int>(nullable: false),
                    OverallSubscribers = table.Column<int>(nullable: false),
                    AverageCostOfSubscriber = table.Column<int>(nullable: false),
                    OverallConvercy = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(nullable: true),
                    Views = table.Column<int>(nullable: false),
                    Subscribers = table.Column<int>(nullable: false),
                    Cost = table.Column<int>(nullable: false),
                    Convercy = table.Column<int>(nullable: false),
                    SingleSubscriberCost = table.Column<int>(nullable: false),
                    ChannelId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ChannelId",
                table: "Posts",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Channels");
        }
    }
}
