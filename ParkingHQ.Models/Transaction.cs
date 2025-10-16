using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [DisplayName("Transaction Date")]

        public DateTime TransactionDate { get; set; }


        public PermanentTenant? PermanentTenant { get; set; }

        [DisplayName("Parkinglot")]

        public ParkingLot ParkingLot { get; set; }

        public decimal Amount { get; set; }

    }

}
