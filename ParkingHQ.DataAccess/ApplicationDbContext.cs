using ParkingHQ.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingHQ.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<CarPark> CarParks { get; set; }
        
        public DbSet<CarParkFloor> CarParkFloors { get; set; }

        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<ParkingLot> ParkingLots { get; set; }

        public DbSet<PermanentTenant> PermanentTenants { get; set; }

        public DbSet<PermanentTenantParkingLot> PermanentTenantParkingLots { get; set; }

        public DbSet<WeekdayTariffSection> WeekdayTariff { get; set; }
        public DbSet<HolidayTariffSection> HolidayTariff { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<EntryExit> EntryExits { get; set; }


    }
}
