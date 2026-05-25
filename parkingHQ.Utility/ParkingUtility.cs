using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;


namespace ParkingHQ.Utility
{
    public class ParkingUtility
    {
        private const int TariffBlockMinutes = 15;
        private const int BlocksPerHour = 60 / TariffBlockMinutes;
        private const int TicketIdMin = 1000;
        private const int TicketIdMax = 9999;

        private readonly IUnitOfWork _unitOfWork;

        public ParkingUtility(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ParkingLot FindFreeParkingLot(CarPark carPark)
        {
            CarParkFloor floorToUse = new CarParkFloor();
            int occupationRate = 0;

            foreach (var floor in carPark.Floors)
            {
                int freeParkingSpaces = floor.ParkingLots.Count(p => !p.IsOccupied && !p.IsPermanentTenant);
                int totalParkingSpaces = floor.ParkingLots.Count();
                int floorOccupationRate = (freeParkingSpaces * 100) / totalParkingSpaces;

                if (floorOccupationRate > occupationRate)
                {
                    occupationRate = floorOccupationRate;
                    floorToUse = floor;
                }
            }

            return floorToUse.ParkingLots.FirstOrDefault(p => !p.IsOccupied && !p.IsPermanentTenant);
        }

        public async Task<decimal> CalcTariffAsync(Ticket ticket)
        {
            CarPark carPark = await _unitOfWork.CarPark.LoadByParkingLot(ticket.ParkingLot.Id);
            carPark = await _unitOfWork.CarPark.LoadWithTariffData(carPark.Id);

            DateTime chargableEntryTime = GetChargableEntryTime(ticket.EntryTime);
            DateTime chargableExitTime = GetChargableExitTime(ticket.ExitTime);

            decimal hours = (decimal)(chargableExitTime - chargableEntryTime).TotalHours;

            if (hours > 24)
            {
                decimal days = Math.Ceiling(hours / 24.0m);
                return carPark.DailyTariff * days;
            }

            return CalcHourTariff(carPark, ticket);
        }

        private decimal CalcHourTariff(CarPark carPark, Ticket ticket)
        {
            if (ticket.EntryTime.Date != ticket.ExitTime.Date)
            {
                throw new NotSupportedException("Overnight parking spanning midnight is not yet supported.");
            }

            if (ticket.EntryTime.DayOfWeek == DayOfWeek.Saturday || ticket.EntryTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return CalcTariffsHoliday(
                    GetChargableEntryTime(ticket.EntryTime),
                    GetChargableExitTime(ticket.ExitTime),
                    carPark.HolidayTariff);
            }

            foreach (var holiday in carPark.Holidays)
            {
                if (ticket.EntryTime.Date == holiday.HolidayDate.Date)
                {
                    return CalcTariffsHoliday(
                        GetChargableEntryTime(ticket.EntryTime),
                        GetChargableExitTime(ticket.ExitTime),
                        carPark.HolidayTariff);
                }
            }

            return CalcTariffsWeekday(
                GetChargableEntryTime(ticket.EntryTime),
                GetChargableExitTime(ticket.ExitTime),
                carPark.WeekdayTariff);
        }

        private DateTime GetChargableEntryTime(DateTime entryTime)
        {
            int roundedMinutes = (entryTime.Minute / TariffBlockMinutes) * TariffBlockMinutes;
            return new DateTime(entryTime.Year, entryTime.Month, entryTime.Day, entryTime.Hour, roundedMinutes, 0);
        }

        private DateTime GetChargableExitTime(DateTime exitTime)
        {
            int roundedMinutes = (exitTime.Minute / TariffBlockMinutes) * TariffBlockMinutes;
            if (roundedMinutes < exitTime.Minute)
            {
                roundedMinutes += TariffBlockMinutes;
            }

            if (roundedMinutes == 60)
            {
                roundedMinutes = 0;
                exitTime = exitTime.AddHours(1);
            }

            return new DateTime(exitTime.Year, exitTime.Month, exitTime.Day, exitTime.Hour, roundedMinutes, 0);
        }

        private decimal CalcTariffsWeekday(DateTime entry, DateTime exit, IEnumerable<WeekdayTariffSection> sections)
        {
            var sectionList = sections.OrderBy(x => x.TariffEndTime).ToList();
            decimal tarif = 0;
            int tarifSegment = 0;
            var currentTime = new DateTime(1, 1, 1, entry.Hour, entry.Minute, 0);
            var exitTime = new DateTime(1, 1, 1, exit.Hour, exit.Minute, 0);

            while (currentTime < exitTime)
            {
                if (tarifSegment >= sectionList.Count)
                    throw new InvalidOperationException("No tariff section configured for this time period. Check the weekday tariff setup.");

                if (currentTime.Hour >= sectionList[tarifSegment].TariffEndTime.Hour)
                {
                    tarifSegment++;
                    continue;
                }

                tarif += sectionList[tarifSegment].TariffPrice / BlocksPerHour;
                currentTime = currentTime.AddMinutes(TariffBlockMinutes);
            }

            return tarif;
        }

        private decimal CalcTariffsHoliday(DateTime entry, DateTime exit, IEnumerable<HolidayTariffSection> sections)
        {
            var sectionList = sections.OrderBy(x => x.TariffEndTime).ToList();
            decimal tarif = 0;
            int tarifSegment = 0;
            var currentTime = new DateTime(1, 1, 1, entry.Hour, entry.Minute, 0);
            var exitTime = new DateTime(1, 1, 1, exit.Hour, exit.Minute, 0);

            while (currentTime < exitTime)
            {
                if (tarifSegment >= sectionList.Count)
                    throw new InvalidOperationException("No tariff section configured for this time period. Check the holiday tariff setup.");

                if (currentTime.Hour >= sectionList[tarifSegment].TariffEndTime.Hour)
                {
                    tarifSegment++;
                    continue;
                }

                tarif += sectionList[tarifSegment].TariffPrice / BlocksPerHour;
                currentTime = currentTime.AddMinutes(TariffBlockMinutes);
            }

            return tarif;
        }

        public async Task<Ticket> GetNewTicket(int carParkId)
        {
            CarPark carPark = await _unitOfWork.CarPark.LoadWithFloors(carParkId);
            List<Ticket> tickets = await _unitOfWork.Ticket.GetTicketsFromCarParkAsync(carParkId);

            int ticketId = Random.Shared.Next(TicketIdMin, TicketIdMax);
            while (tickets.Any(t => t.TicketId == ticketId && t.ExitTime == DateTime.MinValue))
            {
                ticketId = Random.Shared.Next(TicketIdMin, TicketIdMax);
            }

            var parkingLot = FindFreeParkingLot(carPark);
            parkingLot.IsOccupied = true;
            _unitOfWork.ParkingLot.Update(parkingLot);
            await _unitOfWork.Save();

            Ticket newTicket = new Ticket
            {
                ParkingLot = parkingLot,
                TicketId = ticketId,
                EntryTime = DateTime.Now
            };

            await _unitOfWork.Ticket.Add(newTicket);
            await _unitOfWork.Save();

            return newTicket;
        }
    }
}
