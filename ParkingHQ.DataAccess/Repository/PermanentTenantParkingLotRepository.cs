using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class PermanentTenantParkingLotRepository : Repository<PermanentTenantParkingLot>, IPermanentTenantParkingLotRepository
    {
        private ApplicationDbContext _db;

        public PermanentTenantParkingLotRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(PermanentTenantParkingLot obj)
        {
            _db.PermanentTenantParkingLots.Update(obj);
        }

        public PermanentTenantParkingLot GetWithPropertysByPin(int Pin)
        {
            var result = _db.PermanentTenantParkingLots.Include(x => x.ParkingLot).SingleOrDefault(x => x.Pin == Pin);
            return result;
        }

        public PermanentTenantParkingLot GetWithPropertysById(int id)
        {
            var result = _db.PermanentTenantParkingLots.Include(x => x.ParkingLot).SingleOrDefault(x => x.Id == id);
            return result;
        }
    }
}
