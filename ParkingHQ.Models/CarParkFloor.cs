using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public  class CarParkFloor
    {
        public CarParkFloor()
        {
            ParkingLots = new HashSet<ParkingLot>();
        }

        [Key]
        public int Id { get; set; }

        [DisplayName("Floor Name")]
        public string Name { get; set; }

        public ICollection<ParkingLot> ParkingLots { get; set; }

    }
}
