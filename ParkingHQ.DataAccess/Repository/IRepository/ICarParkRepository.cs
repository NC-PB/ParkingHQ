using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface ICarParkRepository : IRepository<CarPark>
    {
        void Update(CarPark obj);

        Task<CarPark> LoadWithFloors(int Id);
        Task<CarPark> LoadByParkingLot(int id);
        Task<CarPark> LoadWithTariffData(int id);

        Task<IEnumerable<CarPark>> LoadAllComplete();

        Task<CarPark> GetTransactions(int Id);

    }
}
