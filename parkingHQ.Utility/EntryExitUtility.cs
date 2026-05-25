using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Utility
{
    public class EntryExitUtility
    {
        private readonly IUnitOfWork _unitOfWork;

        public EntryExitUtility(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddVisitorEntry(int parkingLotId)
        {
            await EntryExitVisitor(parkingLotId, EntryExitType.Entry);
        }

        public async Task AddVisitorExit(int parkingLotId)
        {
            await EntryExitVisitor(parkingLotId, EntryExitType.Exit);
        }

        private async Task EntryExitVisitor(int parkingLotId, EntryExitType entryExitType)
        {
            ParkingLot lot = await _unitOfWork.ParkingLot.GetFirstOrDefault(x => x.Id == parkingLotId);
            CarPark carPark = await _unitOfWork.CarPark.LoadByParkingLot(parkingLotId);

            EntryExit entryExit = new EntryExit
            {
                EntryExitType = entryExitType,
                DateAndTime = DateTime.Now,
                ParkingLot = lot,
                PermanentTenant = null,
                CarPark = carPark
            };

            await _unitOfWork.EntryExit.Add(entryExit);
            await _unitOfWork.Save();
        }

        #region tenant

        public async Task AddTenantEntry(int parkingLotId)
        {
            await EntryExitTenant(parkingLotId, EntryExitType.Entry);
        }

        public async Task AddTenantExit(int parkingLotId)
        {
            await EntryExitTenant(parkingLotId, EntryExitType.Exit);
        }

        private async Task EntryExitTenant(int parkingLotId, EntryExitType entryExitType)
        {
            ParkingLot lot = await _unitOfWork.ParkingLot.GetFirstOrDefault(x => x.Id == parkingLotId);
            PermanentTenant tenant = await _unitOfWork.PermanentTenant.GetPermanentTenantByParkingLotId(parkingLotId);
            CarPark carPark = await _unitOfWork.CarPark.LoadByParkingLot(parkingLotId);

            EntryExit entryExit = new EntryExit
            {
                EntryExitType = entryExitType,
                DateAndTime = DateTime.Now,
                ParkingLot = lot,
                PermanentTenant = tenant,
                CarPark = carPark
            };

            await _unitOfWork.EntryExit.Add(entryExit);
            await _unitOfWork.Save();
        }

        #endregion
    }
}
