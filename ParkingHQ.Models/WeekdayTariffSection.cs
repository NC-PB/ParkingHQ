using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class WeekdayTariffSection
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Tariff end time")]
        public DateTime TariffEndTime { get; set; }

        [DisplayName("Price of section")]
        public decimal TariffPrice { get; set; }
    }
}
