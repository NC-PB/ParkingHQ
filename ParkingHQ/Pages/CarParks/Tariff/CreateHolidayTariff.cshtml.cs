using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingHQ.DataAccess;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks.Tariff
{
    public class CreateHolidayTariffModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;


        public CreateHolidayTariffModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CarPark currentCarPark { get; set; }

        [BindProperty]
        public IEnumerable<HolidayTariffSection> sections { get; set; }

        [BindProperty]
        public HolidayTariffSection TariffSectionHoliday { get; set; }

        [BindProperty]
        public TimeOnly TariffSectionHolidayOnlyTime { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            currentCarPark = await _unitOfWork.CarPark.LoadWithTariffData(id);

            if (currentCarPark.HolidayTariff != null)
            {

                sections = currentCarPark.HolidayTariff.OrderBy(t => t.TariffEndTime).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var carPark = await _unitOfWork.CarPark.GetFirstOrDefault(c => c.Id == currentCarPark.Id);
            TariffSectionHoliday.TariffEndTime += TariffSectionHolidayOnlyTime.ToTimeSpan();
            carPark.HolidayTariff.Add(TariffSectionHoliday);
            _unitOfWork.CarPark.Update(carPark);
            _unitOfWork.Save();

            //Redirect to the same page with the same id




            return RedirectToPage("CreateHolidayTariff", new { id = currentCarPark.Id});

            //return Page();

           // return RedirectToPage("CreateDaily", new {Id = });
        }


        public async Task<IActionResult> OnPostView(int id, int CarparkId)
        {
            var tarifToDelete = _unitOfWork.HolidayTariff.GetFirstOrDefault(x => x.Id == id).Result;
            _unitOfWork.HolidayTariff.Remove(tarifToDelete);
            _unitOfWork.Save();
            return RedirectToPage("CreateHolidayTariff", new { id = CarparkId });
        }


    }
}
