using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IWeekdayTariffSectionRepository : IRepository<WeekdayTariffSection>
    {
        void Update(WeekdayTariffSection obj);
    }
}
