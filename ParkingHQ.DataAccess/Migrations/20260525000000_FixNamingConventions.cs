using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using ParkingHQ.DataAccess;

#nullable disable

namespace ParkingHQ.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260525000000_FixNamingConventions")]
    public partial class FixNamingConventions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ParkingLots: ParkinLotNumber -> ParkingLotNumber
            migrationBuilder.RenameColumn(
                name: "ParkinLotNumber",
                table: "ParkingLots",
                newName: "ParkingLotNumber");

            // EntryExits: ParkinglotId -> ParkingLotId
            migrationBuilder.RenameColumn(
                name: "ParkinglotId",
                table: "EntryExits",
                newName: "ParkingLotId");
            migrationBuilder.RenameIndex(
                name: "IX_EntryExits_ParkinglotId",
                table: "EntryExits",
                newName: "IX_EntryExits_ParkingLotId");

            // EntryExits: carParkId -> CarParkId
            migrationBuilder.RenameColumn(
                name: "carParkId",
                table: "EntryExits",
                newName: "CarParkId");
            migrationBuilder.RenameIndex(
                name: "IX_EntryExits_carParkId",
                table: "EntryExits",
                newName: "IX_EntryExits_CarParkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParkingLotNumber",
                table: "ParkingLots",
                newName: "ParkinLotNumber");

            migrationBuilder.RenameColumn(
                name: "ParkingLotId",
                table: "EntryExits",
                newName: "ParkinglotId");
            migrationBuilder.RenameIndex(
                name: "IX_EntryExits_ParkingLotId",
                table: "EntryExits",
                newName: "IX_EntryExits_ParkinglotId");

            migrationBuilder.RenameColumn(
                name: "CarParkId",
                table: "EntryExits",
                newName: "carParkId");
            migrationBuilder.RenameIndex(
                name: "IX_EntryExits_CarParkId",
                table: "EntryExits",
                newName: "IX_EntryExits_carParkId");
        }
    }
}
