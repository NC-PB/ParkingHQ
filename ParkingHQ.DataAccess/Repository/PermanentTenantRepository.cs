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
    internal class PermanentTenantRepository : Repository<PermanentTenant>, IPermanentTenantRepository
    {
        private ApplicationDbContext _db;

        public PermanentTenantRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(PermanentTenant obj)
        {
            _db.PermanentTenants.Update(obj);
        }


        public async Task<PermanentTenant> GetPermanentTenantByParkingLotId(int id)
        { 
            var result = await _db.PermanentTenants.Where(x => x.PermanentTenantParkingLots.Any(x => x.ParkingLot.Id == id)).SingleOrDefaultAsync();
            return result;
        }

        public async Task<PermanentTenant> GetWithPropertiesById(int id)
        { 
            var tenant = await _db.PermanentTenants.Where(x => x.Id == id)
                .Include(x => x.PermanentTenantParkingLots)
                .ThenInclude(x => x.ParkingLot)
                .SingleOrDefaultAsync();

            return tenant;
        }
         

    }
}
