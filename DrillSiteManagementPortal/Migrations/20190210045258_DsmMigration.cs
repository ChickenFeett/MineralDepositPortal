using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DrillSiteManagementPortal.Migrations
{
    public partial class DsmMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Config",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumberOfRecordsToQueryDip = table.Column<int>(nullable: false),
                    DipMarginOfError = table.Column<int>(nullable: false),
                    NumberOfRecordsToQueryAzimuth = table.Column<int>(nullable: false),
                    AzimuthMarginOfError = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Config", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrillSites",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    CollarAzimuth = table.Column<double>(nullable: false),
                    CollarDip = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillSites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DepthReadings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dip = table.Column<double>(nullable: false),
                    Azimuth = table.Column<double>(nullable: false),
                    TrustWorthiness = table.Column<double>(nullable: false),
                    DrillSiteModelId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepthReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepthReadings_DrillSites_DrillSiteModelId",
                        column: x => x.DrillSiteModelId,
                        principalTable: "DrillSites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepthReadings_DrillSiteModelId",
                table: "DepthReadings",
                column: "DrillSiteModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Config");

            migrationBuilder.DropTable(
                name: "DepthReadings");

            migrationBuilder.DropTable(
                name: "DrillSites");
        }
    }
}
