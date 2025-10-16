using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks.Floors
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<CarParkFloor> Floors { get; set; }

        [BindProperty]
        public CarParkFloor Floor { get; set; }

        [BindProperty]
        public int ParkingLotsCount { get; set; }

        [BindProperty]
        public CarPark CarPark { get; set; }
        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGet(int Id)
        {
            // Read all car parks and display them in a table
            CarPark = await _unitOfWork.CarPark.GetFirstOrDefault(x => x.Id == Id);
            Floors = CarPark.Floors;
        }


        public async Task<IActionResult> OnPost()
        {
            int Id = CarPark.Id;
            CarPark = await _unitOfWork.CarPark.GetFirstOrDefault(x => x.Id == Id);

            for (int i = 0; i < ParkingLotsCount; i++)
            {
                int LotNumber = i + 1;
                Floor.ParkingLots.Add(new ParkingLot() { IsOccupied = false, IsPermanentTenant = false, ParkinLotNumber = LotNumber}); ;
            }


            CarPark.Floors.Add(Floor);
             _unitOfWork.CarPark.Update(CarPark);
            await _unitOfWork.Save();
            return RedirectToPage("Index", new {id = CarPark.Id});
        }
    }
}
