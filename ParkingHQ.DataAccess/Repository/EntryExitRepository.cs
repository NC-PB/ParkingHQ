using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class EntryExitRepository : Repository<EntryExit>, IEntryExitRepository
    {
        private ApplicationDbContext _db;

        public EntryExitRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(EntryExit obj)
        {
            _db.EntryExits.Update(obj);
        }

        public async Task<List<EntryExit>> GetEntryExitByCarParkId(int carParkId) 
        {
            var result = await _db.EntryExits.Where(x => x.CarPark.Id == carParkId)
                .Include(x => x.PermanentTenant)
                .Include(x => x.ParkingLot)
                .ToListAsync();
            return result;
        }

    }
}
