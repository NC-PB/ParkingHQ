using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.DataAccess.Repository
{
    internal class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private ApplicationDbContext _db;

        public TicketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Ticket obj)
        {
            _db.Tickets.Update(obj);
        }

        public async Task<List<Ticket>> GetTicketsFromCarParkAsync(int id)
        {
            var park = await _db.CarParks
                .Include(m => m.Floors).ThenInclude(m => m.ParkingLots)
                .SingleOrDefaultAsync(x => x.Id == id);

            var parkingLots = park.Floors.SelectMany(x => x.ParkingLots).ToList();

            return await _db.Tickets
                .Where(x => parkingLots.Contains(x.ParkingLot))
                .ToListAsync();
        }

        public async Task<Ticket> GetTicketByTicketIdAsync(int id)
        {
            return await _db.Tickets
                .Include(m => m.ParkingLot)
                .SingleOrDefaultAsync(x => x.TicketId == id);
        }
    }
}
