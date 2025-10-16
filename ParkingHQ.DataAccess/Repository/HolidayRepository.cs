using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class HolidayRepository : Repository<Holiday>, IHolidayRepository
    {
        private ApplicationDbContext _db;

        public HolidayRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Holiday obj)
        {
            _db.Holidays.Update(obj);
        }
    }
}
