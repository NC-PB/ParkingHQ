using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public  class PermanentTenantParkingLot
    {
        public int Id { get; set; }

        public int Pin { get; set; }

        public DateTime LastPayment { get; set; }

        public ParkingLot ParkingLot { get; set; }


    }
}
