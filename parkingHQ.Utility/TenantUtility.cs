using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingHQ.DataAccess;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Utility
{
    public class TenantUtility
    {
        private readonly IUnitOfWork _unitOfWork;

        public TenantUtility(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int GeneratePermanentTenantPin()
        {
            Random random = new Random();
            int pin = random.Next(10000, 99999);

            List<PermanentTenantParkingLot> permanentTenantParkingLots= _unitOfWork.PermanentTenantParkingLot.GetAll().ToList();

            while (permanentTenantParkingLots.Where(t => t.Pin == pin).Count() > 0)
            {
                pin = random.Next(10000, 99999);
            }
            return pin;
        }


        public bool AuthenticateTenant(int pin, int CarParkId, bool exit = false)
        { 
            PermanentTenantParkingLot permanentTenantParkingLot = _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPin(pin);

            if (permanentTenantParkingLot == null)
                return false;

            CarPark carPark = _unitOfWork.CarPark.GetFirstOrDefault(x => x.Id == CarParkId).Result;
            CarPark carParkFromPin = _unitOfWork.CarPark.LoadByParkingLot(permanentTenantParkingLot.ParkingLot.Id).Result;
            
            if(carPark.Id != carParkFromPin.Id) 
                return false;

            if (permanentTenantParkingLot.ParkingLot.IsOccupied == true && exit == false)
                return false;


            return true;
        }

    }
}
