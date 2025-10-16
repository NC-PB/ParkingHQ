using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class CarParkFloorSelectModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<CarParkFloor> Floors { get; set; }

        [BindProperty]
        public int PermanentTenantId { get; set; }
        public CarParkFloorSelectModel(IUnitOfWork unitOfWork )
        {
                _unitOfWork= unitOfWork;
        }

        public async Task OnGet(int tenant, int carpark)
        {
            CarPark park = await _unitOfWork.CarPark.LoadWithFloors(carpark);
            Floors= park.Floors;
            PermanentTenantId = tenant;
        }
    }
}
