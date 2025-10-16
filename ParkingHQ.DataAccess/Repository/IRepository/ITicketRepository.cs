using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository.IRepository
{
    public interface ITicketRepository : IRepository<Ticket>
    {
        void Update(Ticket obj);

        public List<Ticket> GetTicketsFromCarPark(int Id);

        public Ticket GetTicketByTicketId(int Id);

    }
}
