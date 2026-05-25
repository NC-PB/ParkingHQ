using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class EntryExit
    {
        public int Id { get; set; }

        [DisplayName("Tenant")]
        public PermanentTenant? PermanentTenant { get; set; }

        [DisplayName("Parking Lot")]
        public ParkingLot ParkingLot { get; set; }

        [DisplayName("Date")]
        public DateTime DateAndTime { get; set; }

        [DisplayName("Type")]
        public EntryExitType EntryExitType { get; set; }

        public CarPark CarPark { get; set; }
    }

    public enum EntryExitType
    {
        Entry,
        Exit
    }
}
