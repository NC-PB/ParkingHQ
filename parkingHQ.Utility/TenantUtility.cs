using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Utility
{
    public class TenantUtility
    {
        private const int PinMin = 10000;
        private const int PinMax = 99999;

        private readonly IUnitOfWork _unitOfWork;

        public TenantUtility(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int GeneratePermanentTenantPin()
        {
            List<PermanentTenantParkingLot> existingLots = _unitOfWork.PermanentTenantParkingLot.GetAll().ToList();

            int pin = Random.Shared.Next(PinMin, PinMax);
            while (existingLots.Any(t => t.Pin == pin))
            {
                pin = Random.Shared.Next(PinMin, PinMax);
            }

            return pin;
        }

        public async Task<bool> AuthenticateTenantAsync(int pin, int carParkId, bool exit = false)
        {
            PermanentTenantParkingLot tenantLot = await _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPinAsync(pin);

            if (tenantLot == null)
                return false;

            CarPark carPark = await _unitOfWork.CarPark.GetFirstOrDefault(x => x.Id == carParkId);
            CarPark carParkFromPin = await _unitOfWork.CarPark.LoadByParkingLot(tenantLot.ParkingLot.Id);

            if (carPark.Id != carParkFromPin.Id)
                return false;

            if (tenantLot.ParkingLot.IsOccupied && !exit)
                return false;

            return true;
        }
    }
}
