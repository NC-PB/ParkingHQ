using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NuGet.Packaging;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks.Floors
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<CarParkFloor> Floors { get; set; }

        [BindProperty]
        public CarPark CarPark { get; set; }
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGet(int Id)
        {
            // Read all car parks and display them in a table
            //CarPark = await _unitOfWork.CarPark.GetFirstOrDefault(x => x.Id == Id);
            CarPark = await _unitOfWork.CarPark.LoadWithFloors(Id);
            Floors = CarPark.Floors;
        }

        
    }
}
