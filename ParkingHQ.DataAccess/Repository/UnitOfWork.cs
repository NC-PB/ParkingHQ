using ParkingHQ.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CarPark = new CarParkRepository(_db);
            CarParkFloor = new CarParkFloorRepository(_db);
            Holiday = new HolidayRepository(_db);
            ParkingLot = new ParkingLotRepository(_db);
            PermanentTenant = new PermanentTenantRepository(_db);
            PermanentTenantParkingLot = new PermanentTenantParkingLotRepository(_db);
            WeekdayTariff = new WeekdayTariffSectionRepository(_db);
            HolidayTariff = new HolidayTariffSectionRepository(_db);
            Transaction = new TransactionRepository(_db);
            Ticket = new TicketRepository(_db);
            EntryExit = new EntryExitRepository(_db);
        }

        public ICarParkRepository CarPark { get; private set; }
        public ICarParkFloorRepository CarParkFloor { get; private set; }

        public IHolidayRepository Holiday { get; private set; }

        public IParkingLotRepository ParkingLot { get; private set; }

        public IPermanentTenantRepository PermanentTenant { get; private set; }

        public IPermanentTenantParkingLotRepository PermanentTenantParkingLot { get; private set; }

        public IHolidayTariffSectionRepository HolidayTariff { get; private set; }

        public IWeekdayTariffSectionRepository WeekdayTariff { get; private set; }


        public ITransactionRepository Transaction { get; private set; }

        public ITicketRepository Ticket { get; private set; }

        public IEntryExitRepository EntryExit { get; private set; }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
