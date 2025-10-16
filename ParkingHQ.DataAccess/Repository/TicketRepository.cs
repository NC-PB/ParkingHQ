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

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Ticket obj)
        {
            _db.Tickets.Update(obj);
        }


        public List<Ticket> GetTicketsFromCarPark(int Id)
        {
            //Get all Tickets from a CarPark by ID of CarPark
            //var tickets = _db.Tickets.Where(x => x.ParkingLot.CarPark.Id == id).ToList();


            var park = _db.CarParks.Include(m => m.Floors).ThenInclude(m => m.ParkingLots).SingleOrDefaultAsync(x => x.Id == Id).Result;

            //load all parkinglots from park in a list
            var parkingLots = park.Floors.SelectMany(x => x.ParkingLots).ToList();

            //Find all tickets from parkinglots in list
            var tickets = _db.Tickets.Where(x => parkingLots.Contains(x.ParkingLot)).ToList();

            return tickets;
        }

        public Ticket GetTicketByTicketId(int Id)
        {
            var ticket = _db.Tickets
                .Include(m => m.ParkingLot)
                .Where(x => x.TicketId == Id)
                .SingleOrDefaultAsync().Result;
            return ticket;
        }


    }
}
