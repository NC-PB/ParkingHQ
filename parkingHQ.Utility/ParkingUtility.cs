using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;


namespace ParkingHQ.Utility
{
    public class ParkingUtility
    {
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
                int freeParkingSpaces = floor.ParkingLots.Where(p => p.IsOccupied == false && !p.IsPermanentTenant).Count();
                int totalParkingSpaces = floor.ParkingLots.Count();

                int floorOccupationRate = (freeParkingSpaces * 100) / totalParkingSpaces;

                if (floorOccupationRate > occupationRate)
                {
                    occupationRate = floorOccupationRate;
                    floorToUse= floor;
                }
            }
           
            ParkingLot parkingLotToUse = floorToUse.ParkingLots.Where(p => p.IsOccupied == false && !p.IsPermanentTenant).FirstOrDefault();

            return parkingLotToUse;

        }

        public int GenerateTicketId(List<Ticket> tickets)
        { 

            Random random = new Random();
            int ticketId = random.Next(1000, 9999);

            while(tickets.Where(t => t.TicketId == ticketId && t.ExitTime == DateTime.MinValue).Count() > 0)
            {
                ticketId = random.Next(1000, 9999);
            }

            return ticketId;
        }

        public decimal CalcTariffs(Ticket ticket)
        {
            //Load CarPark with TariffData
            //
            CarPark carPark = _unitOfWork.CarPark.LoadByParkingLot(ticket.ParkingLot.Id).Result;
            carPark = _unitOfWork.CarPark.LoadWithTariffData(carPark.Id).Result;


            //Calculate chargable entry and exit time in 15min blocks
            //
            DateTime chargableEntryTime = GetChargabelEntryTime(ticket.EntryTime);
            DateTime chargableExitTime = GetChargabelExitTime(ticket.ExitTime);

 

            decimal hours = (decimal)(chargableExitTime - chargableEntryTime).TotalHours;
            if (hours > 24)
            {
                //Calculate daily tariff
                decimal tariff = carPark.DailyTariff;
                
                var days = hours / 24.0m;

                days = Math.Ceiling(days);

                decimal result = tariff * days;
                return result;

            }
            else
            {
                var result = CalcHourTariff(carPark, ticket);
                return result;
            }
        }

        private decimal CalcHourTariff(CarPark carPark, Ticket ticket)
        {
           
            if(ticket.EntryTime.Date != ticket.ExitTime.Date) 
            {
                
            }

            //Check if it is a weekend
            //
            if (ticket.EntryTime.Date.DayOfWeek == DayOfWeek.Saturday || ticket.EntryTime.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                return CalcTariffsHoliiday(GetChargabelEntryTime(ticket.EntryTime), GetChargabelExitTime(ticket.ExitTime), carPark.HolidayTariff);
            }

            //Check if it is a Holiday
            //
            foreach (var holiday in carPark.Holidays) 
            {
                if (ticket.EntryTime.Date == holiday.HolidayDate.Date)
                {
                    return CalcTariffsHoliiday(GetChargabelEntryTime(ticket.EntryTime), GetChargabelExitTime(ticket.ExitTime), carPark.HolidayTariff);
                }
            }

            //We have a regular weekday
            //
            return CalcTariffsWeekday(GetChargabelEntryTime(ticket.EntryTime), GetChargabelExitTime(ticket.ExitTime), carPark.WeekdayTariff);
        }

        DateTime GetChargabelEntryTime(DateTime entryTime)
        {
            int roundedMinutes = (entryTime.Minute / 15) * 15;
            if (roundedMinutes > entryTime.Minute)
            {
                roundedMinutes -= 15;
            }

            return new DateTime(entryTime.Year, entryTime.Month, entryTime.Day, entryTime.Hour, roundedMinutes, 0);
        }

        DateTime GetChargabelExitTime(DateTime exitTime)
        {


            int roundedMinutes = (exitTime.Minute / 15) * 15;
            if (roundedMinutes < exitTime.Minute)
            {
                roundedMinutes += 15;
            }

            if(roundedMinutes == 60)
            {
                roundedMinutes = 0;
                exitTime = exitTime.AddHours(1);
            }

            var test = new DateTime(exitTime.Year, exitTime.Month, exitTime.Day, exitTime.Hour, roundedMinutes, 0);

            return test;
        }


        decimal CalcTariffsWeekday(DateTime entry, DateTime exit, IEnumerable<WeekdayTariffSection> sections)
        {

            sections = sections.OrderBy(x => x.TariffEndTime);



            decimal tarif = 0;

            int tarifSegment = 0;

            var currentTime = new DateTime(1, 1, 1, entry.Hour, entry.Minute, 0);
            var extiTime = new DateTime(1, 1, 1, exit.Hour, exit.Minute, 0);

            while (currentTime < extiTime)
            {
                if (currentTime.Hour > sections.ElementAt(tarifSegment).TariffEndTime.Hour)
                {
                    tarifSegment++;
                    continue;
                }

                tarif = tarif + (sections.ElementAt(tarifSegment).TariffPrice / 4);
                currentTime = currentTime.AddMinutes(15);
            }
            return tarif;

        }

        decimal CalcTariffsHoliiday(DateTime entry, DateTime exit, IEnumerable<HolidayTariffSection> sections)
        {

            sections = sections.OrderBy(x => x.TariffEndTime);



            decimal tarif = 0;

            int tarifSegment = 0;

            var currentTime = new DateTime(1, 1, 1, entry.Hour, entry.Minute, 0);
            var extiTime = new DateTime(1, 1, 1, exit.Hour, exit.Minute, 0);


            while (currentTime < extiTime)
            {
                if (currentTime.Hour > sections.ElementAt(tarifSegment).TariffEndTime.Hour)
                {
                    tarifSegment++;
                    continue;
                }

                tarif = tarif + (sections.ElementAt(tarifSegment).TariffPrice / 4);
                currentTime = currentTime.AddMinutes(15);
            }
            return tarif;

        }


        public async Task<Ticket> GetNewTicket(int CarParkId)
        {
            

            CarPark carPark = await  _unitOfWork.CarPark.LoadWithFloors(CarParkId);

            List<Ticket> tickets = _unitOfWork.Ticket.GetTicketsFromCarPark(CarParkId);

            var ticketId = GenerateTicketId(tickets);
            var parkingLot = FindFreeParkingLot(carPark);



            parkingLot.IsOccupied = true;

            _unitOfWork.ParkingLot.Update(parkingLot);
            await _unitOfWork.Save();



            Ticket NewTicket = new Ticket();
            NewTicket.ParkingLot = parkingLot;
            NewTicket.TicketId = ticketId;
            NewTicket.EntryTime = DateTime.Now;
            await _unitOfWork.Ticket.Add(NewTicket);
            await _unitOfWork.Save();

            return NewTicket;
        }

    }
}
