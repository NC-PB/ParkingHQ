using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {

        ICarParkRepository CarPark { get; }

        ICarParkFloorRepository CarParkFloor { get; }

        IHolidayRepository Holiday { get; }

        IParkingLotRepository ParkingLot { get; }

        IPermanentTenantRepository PermanentTenant { get; }

        IPermanentTenantParkingLotRepository PermanentTenantParkingLot { get; }

        IHolidayTariffSectionRepository HolidayTariff{ get; }
        IWeekdayTariffSectionRepository WeekdayTariff { get; }

        ITransactionRepository Transaction { get; }

        ITicketRepository Ticket { get; }

        IEntryExitRepository EntryExit { get; }

        Task Save();

    }
}
