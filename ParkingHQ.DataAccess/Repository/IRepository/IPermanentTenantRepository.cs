using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IPermanentTenantRepository : IRepository<PermanentTenant>
    {
        void Update(PermanentTenant obj);

        public Task<PermanentTenant> GetPermanentTenantByParkingLotId(int id);

        public Task<PermanentTenant> GetWithPropertiesById(int id);


    }
}
