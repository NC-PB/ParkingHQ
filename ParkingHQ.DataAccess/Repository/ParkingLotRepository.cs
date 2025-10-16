using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class ParkingLotRepository : Repository<ParkingLot>, IParkingLotRepository
    {
        private ApplicationDbContext _db;

        public ParkingLotRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ParkingLot obj)
        {
            _db.ParkingLots.Update(obj);
        }
    }
}
