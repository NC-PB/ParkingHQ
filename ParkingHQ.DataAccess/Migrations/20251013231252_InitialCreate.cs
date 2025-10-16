using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingHQ.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarParks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DailyTariff = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermanentTenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentTenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarParkFloors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarParkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParkFloors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarParkFloors_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HolidayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HolidayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CarParkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holidays_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HolidayTariff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TariffEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TariffPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarParkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayTariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayTariff_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeekdayTariff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TariffEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TariffPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarParkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekdayTariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekdayTariff_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ParkingLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParkinLotNumber = table.Column<int>(type: "int", nullable: false),
                    IsOccupied = table.Column<bool>(type: "bit", nullable: false),
                    IsPermanentTenant = table.Column<bool>(type: "bit", nullable: false),
                    CarParkFloorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingLots_CarParkFloors_CarParkFloorId",
                        column: x => x.CarParkFloorId,
                        principalTable: "CarParkFloors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EntryExits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermanentTenantId = table.Column<int>(type: "int", nullable: true),
                    ParkinglotId = table.Column<int>(type: "int", nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntryExitType = table.Column<int>(type: "int", nullable: false),
                    carParkId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryExits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryExits_CarParks_carParkId",
                        column: x => x.carParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryExits_ParkingLots_ParkinglotId",
                        column: x => x.ParkinglotId,
                        principalTable: "ParkingLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryExits_PermanentTenants_PermanentTenantId",
                        column: x => x.PermanentTenantId,
                        principalTable: "PermanentTenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PermanentTenantParkingLots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pin = table.Column<int>(type: "int", nullable: false),
                    LastPayment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingLotId = table.Column<int>(type: "int", nullable: false),
                    PermanentTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermanentTenantParkingLots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermanentTenantParkingLots_ParkingLots_ParkingLotId",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermanentTenantParkingLots_PermanentTenants_PermanentTenantId",
                        column: x => x.PermanentTenantId,
                        principalTable: "PermanentTenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ParkingLotId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_ParkingLots_ParkingLotId",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PermanentTenantId = table.Column<int>(type: "int", nullable: true),
                    ParkingLotId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarParkId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_CarParks_CarParkId",
                        column: x => x.CarParkId,
                        principalTable: "CarParks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_ParkingLots_ParkingLotId",
                        column: x => x.ParkingLotId,
                        principalTable: "ParkingLots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_PermanentTenants_PermanentTenantId",
                        column: x => x.PermanentTenantId,
                        principalTable: "PermanentTenants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarParkFloors_CarParkId",
                table: "CarParkFloors",
                column: "CarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryExits_carParkId",
                table: "EntryExits",
                column: "carParkId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryExits_ParkinglotId",
                table: "EntryExits",
                column: "ParkinglotId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryExits_PermanentTenantId",
                table: "EntryExits",
                column: "PermanentTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Holidays_CarParkId",
                table: "Holidays",
                column: "CarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTariff_CarParkId",
                table: "HolidayTariff",
                column: "CarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingLots_CarParkFloorId",
                table: "ParkingLots",
                column: "CarParkFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_PermanentTenantParkingLots_ParkingLotId",
                table: "PermanentTenantParkingLots",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_PermanentTenantParkingLots_PermanentTenantId",
                table: "PermanentTenantParkingLots",
                column: "PermanentTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParkingLotId",
                table: "Tickets",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CarParkId",
                table: "Transactions",
                column: "CarParkId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ParkingLotId",
                table: "Transactions",
                column: "ParkingLotId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PermanentTenantId",
                table: "Transactions",
                column: "PermanentTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekdayTariff_CarParkId",
                table: "WeekdayTariff",
                column: "CarParkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntryExits");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "HolidayTariff");

            migrationBuilder.DropTable(
                name: "PermanentTenantParkingLots");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "WeekdayTariff");

            migrationBuilder.DropTable(
                name: "ParkingLots");

            migrationBuilder.DropTable(
                name: "PermanentTenants");

            migrationBuilder.DropTable(
                name: "CarParkFloors");

            migrationBuilder.DropTable(
                name: "CarParks");
        }
    }
}
