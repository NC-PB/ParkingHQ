using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class HolidayTariffSection
    {
        [Key]
        public int Id { get; set; }
    
        public DateTime TariffEndTime { get; set; }

        public decimal TariffPrice { get; set; }
    }
}
