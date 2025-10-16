using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IPermanentTenantParkingLotRepository : IRepository<PermanentTenantParkingLot>
    {
        void Update(PermanentTenantParkingLot obj);

        public PermanentTenantParkingLot GetWithPropertysByPin(int Pin);
        public PermanentTenantParkingLot GetWithPropertysById(int id);

    }
}
