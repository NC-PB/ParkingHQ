using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface ICarParkFloorRepository : IRepository<CarParkFloor>
    {
        void Update(CarParkFloor obj);

        Task<CarParkFloor> LoadWithLots(int Id);
        Task<CarParkFloor> LoadByParkingLotId(int id);
    }
}
