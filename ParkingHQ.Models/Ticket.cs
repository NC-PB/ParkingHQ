using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        public int TicketId { get; set; }

        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }

        public ParkingLot ParkingLot { get; set; }

        public decimal Price { get; set; }
    }
}
