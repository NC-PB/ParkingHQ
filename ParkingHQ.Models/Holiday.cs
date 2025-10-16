using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingHQ.Models
{
    public class Holiday
    {
        public int Id { get; set; }

        [DisplayName("Holiday Name")]
        public String HolidayName { get; set; }


        [DisplayName("Date")]
        public DateTime HolidayDate { get; set; }

    }
}
