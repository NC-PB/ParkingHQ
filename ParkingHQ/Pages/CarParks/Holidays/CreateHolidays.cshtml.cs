using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks.Holidays
{
    public class CreateHolidaysModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateHolidaysModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public IEnumerable<Holiday> Holidays { get; set; }

        [BindProperty]
        public CarPark currentCarPark { get; set; }

        [BindProperty]
        public Holiday NewHoliday { get; set; }

        [BindProperty]
        public DateOnly HolidayDateOnly { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var carPark = await _unitOfWork.CarPark.LoadWithTariffData(id);
            currentCarPark = carPark;
            Holidays = carPark.Holidays.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var carPark = await _unitOfWork.CarPark.GetFirstOrDefault(c => c.Id == currentCarPark.Id);
            NewHoliday.HolidayDate = HolidayDateOnly.ToDateTime(TimeOnly.MinValue);
            carPark.Holidays.Add(NewHoliday);
            _unitOfWork.CarPark.Update(carPark);
            await _unitOfWork.Save();
            return RedirectToPage("CreateHolidays", new { id = currentCarPark.Id });
        }

        public async Task<IActionResult> OnPostViewAsync(int id, int CarparkId)
        {
            var holidayToDelete = await _unitOfWork.Holiday.GetFirstOrDefault(x => x.Id == id);
            _unitOfWork.Holiday.Remove(holidayToDelete);
            await _unitOfWork.Save();
            return RedirectToPage("CreateHolidays", new { id = CarparkId });
        }
    }
}
