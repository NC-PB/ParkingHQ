using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface IEntryExitRepository : IRepository<EntryExit>
    {
        void Update(EntryExit obj);

        Task<List<EntryExit>> GetEntryExitByCarParkId(int carParkId);

    }
}
