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
    public class CreateWeekdayTariffModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;


        public CreateWeekdayTariffModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CarPark currentCarPark { get; set; }

        [BindProperty]
        public IEnumerable<WeekdayTariffSection> sections { get; set; }

        [BindProperty]
        public WeekdayTariffSection TariffSectionDaily { get; set; }

        [BindProperty]
        public TimeOnly TariffSectionDailyOnlyTime { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            currentCarPark = await _unitOfWork.CarPark.LoadWithTariffData(id);

            if (currentCarPark.WeekdayTariff != null)
            {

                sections = currentCarPark.WeekdayTariff.OrderBy(t => t.TariffEndTime).ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var carPark = await _unitOfWork.CarPark.GetFirstOrDefault(c => c.Id == currentCarPark.Id);
            TariffSectionDaily.TariffEndTime += TariffSectionDailyOnlyTime.ToTimeSpan();
            carPark.WeekdayTariff.Add(TariffSectionDaily);
            _unitOfWork.CarPark.Update(carPark);
            _unitOfWork.Save();

            return RedirectToPage("CreateWeekdayTariff", new { id = currentCarPark.Id});
        }


        public async Task<IActionResult> OnPostView(int id, int CarparkId)
        {
            var tarifToDelete = _unitOfWork.WeekdayTariff.GetFirstOrDefault(x => x.Id == id).Result;
            _unitOfWork.WeekdayTariff.Remove(tarifToDelete);
            _unitOfWork.Save();
            return RedirectToPage("CreateWeekdayTariff", new { id = CarparkId });
        }






    }
}
