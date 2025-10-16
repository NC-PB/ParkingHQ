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
    internal class CarParkFloorRepository : Repository<CarParkFloor>, ICarParkFloorRepository
    {
        private ApplicationDbContext _db;

        public CarParkFloorRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(CarParkFloor obj)
        {
            _db.CarParkFloors.Update(obj);
        }


        public async Task<CarParkFloor> LoadWithLots(int Id)
        { 
            var park = _db.CarParkFloors.Include(m => m.ParkingLots).SingleOrDefaultAsync(x => x.Id == Id).Result;


            return park;

        }

        public async Task<CarParkFloor> LoadByParkingLotId(int id)
        {
            var carParkFloor = _db.CarParkFloors.Where(x => x.ParkingLots.Any(y => y.Id == id)).SingleOrDefaultAsync().Result;
            return carParkFloor;
        }

    }
}
