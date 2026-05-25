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

        public void Update(PermanentTenantParkingLot obj)
        {
            _db.PermanentTenantParkingLots.Update(obj);
        }

        public async Task<PermanentTenantParkingLot> GetWithPropertysByPinAsync(int pin)
        {
            return await _db.PermanentTenantParkingLots
                .Include(x => x.ParkingLot)
                .SingleOrDefaultAsync(x => x.Pin == pin);
        }

        public async Task<PermanentTenantParkingLot> GetWithPropertysByIdAsync(int id)
        {
            return await _db.PermanentTenantParkingLots
                .Include(x => x.ParkingLot)
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
