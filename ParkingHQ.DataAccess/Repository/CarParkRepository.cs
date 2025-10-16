using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ParkingHQ.DataAccess.Repository
{
    internal class CarParkRepository : Repository<CarPark>, ICarParkRepository
    {
        private ApplicationDbContext _db;

        public CarParkRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(CarPark obj)
        {
            _db.CarParks.Update(obj);

        }

        public async Task<CarPark> LoadWithFloors(int Id)
        {
            var park = await _db.CarParks.Include(m => m.Floors).ThenInclude(m => m.ParkingLots).SingleOrDefaultAsync(x => x.Id == Id);

            //_db.CarParks.Entry(park).Collection(p => p.Floors).Load();
            //_db.CarParks
            return park;
            

           //CarPark park = _db.CarParks.SingleOrDefaultAsync(x => x.Id == Id).Include(x => x.Floors);
        }

        public async Task<CarPark> LoadByParkingLot(int id)
        { 

            //find CarPraky bi ID of ParkingLot
            var carPark = await _db.CarParks.Where(x => x.Floors.Any(y => y.ParkingLots.Any(z => z.Id == id))).SingleOrDefaultAsync();
            return carPark;
        }

        public async Task<CarPark> LoadWithTariffData(int id)
        {


            var carPark = await _db.CarParks.Where(x => x.Id == id)
                .Include(x => x.Holidays)
                .Include(x => x.WeekdayTariff)
                .Include(x => x.HolidayTariff)
                .SingleOrDefaultAsync();

            return carPark;
        }

        public async Task<IEnumerable<CarPark>> LoadAllComplete()
        {
            var park = await _db.CarParks
                .Include(m => m.Floors).ThenInclude(m => m.ParkingLots)
                .Include(x => x.WeekdayTariff)
                .Include(x => x.HolidayTariff)
                .Include(x => x.Holidays)
                .Include(x => x.Transactions).ToListAsync();
                
            //_db.CarParks.Entry(park).Collection(p => p.Floors).Load();
            //_db.CarParks
            return park;


            //CarPark park = _db.CarParks.SingleOrDefaultAsync(x => x.Id == Id).Include(x => x.Floors);
        }

        public async Task<CarPark> GetTransactions(int Id)
        {
            var park = await _db.CarParks
                .Where(x => x.Id == Id)
                .Include(x => x.Transactions)
                    .ThenInclude(x => x.ParkingLot)
                .Include(x => x.Transactions)
                    .ThenInclude(x => x.PermanentTenant)
                .SingleOrDefaultAsync();

            //_db.CarParks.Entry(park).Collection(p => p.Floors).Load();
            //_db.CarParks
            return park;


            //CarPark park = _db.CarParks.SingleOrDefaultAsync(x => x.Id == Id).Include(x => x.Floors);
        }




    }
}
