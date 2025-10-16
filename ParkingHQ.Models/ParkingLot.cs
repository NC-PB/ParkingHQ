using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class ParkingLot
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Parkinglot Number")]
        public int ParkinLotNumber { get; set; }

        [DisplayName("Is Occupied")]

        public bool IsOccupied { get; set; }

        [DisplayName("Is Permanent Tenant")]
        public bool IsPermanentTenant { get; set; }


    }
}
