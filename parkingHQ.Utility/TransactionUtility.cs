using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Utility
{
    public class TransactionUtility
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionUtility(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public async Task TicketPay(Ticket ticket)
        { 
            Transaction trans = new Transaction();
            var carPark = await _unitOfWork.CarPark.LoadByParkingLot(ticket.ParkingLot.Id);
            trans.ParkingLot = ticket.ParkingLot;
            trans.TransactionDate = DateTime.Now;
            trans.Amount = ticket.Price;
            carPark.Transactions.Add(trans);
            _unitOfWork.CarPark.Update(carPark);
            await _unitOfWork.Save();
        }

        public async Task TenantPay(int tenantId, ParkingLot parkingLot)
        {
            Transaction trans = new Transaction();
            var carPark = await _unitOfWork.CarPark.LoadByParkingLot(parkingLot.Id);
            var tenant = await _unitOfWork.PermanentTenant.GetFirstOrDefault(x => x.Id == tenantId);

            trans.ParkingLot = parkingLot;
            trans.TransactionDate = DateTime.Now;
            trans.Amount = carPark.MonthlyRent;
            trans.PermanentTenant = tenant;
            carPark.Transactions.Add(trans);
            _unitOfWork.CarPark.Update(carPark);
            await _unitOfWork.Save();

        }


    }
}
