using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class WeekdayTariffSectionRepository : Repository<WeekdayTariffSection>, IWeekdayTariffSectionRepository
    {
        private ApplicationDbContext _db;

        public WeekdayTariffSectionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(WeekdayTariffSection obj)
        {
            _db.WeekdayTariff.Update(obj);
        }
    }
}
