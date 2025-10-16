using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ParkingHQ.Models
{
    public class CarPark
    {
        public CarPark()
        {
            Floors = new HashSet<CarParkFloor>();

            WeekdayTariff = new HashSet<WeekdayTariffSection>();

            HolidayTariff = new HashSet<HolidayTariffSection>();

            Holidays = new HashSet<Holiday>();

            Transactions = new HashSet<Transaction>();

            EntryExits = new HashSet<EntryExit>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }


        [DisplayName("ZIP Code")]
        public string ZipCode { get; set; }

        [DisplayName("Daily Tariff")]
        public decimal DailyTariff { get; set; }

        [DisplayName("Monthly Rent")]
        public decimal MonthlyRent { get; set; }

        public ICollection<CarParkFloor> Floors { get; set; }
        public ICollection<WeekdayTariffSection> WeekdayTariff { get; set; }

        public ICollection<HolidayTariffSection> HolidayTariff { get; set; }

        public ICollection<Holiday> Holidays { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<EntryExit> EntryExits { get; set; }

    }
}
