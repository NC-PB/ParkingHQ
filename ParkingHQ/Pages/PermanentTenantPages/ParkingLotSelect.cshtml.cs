using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class ParkingLotSelectModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<ParkingLot> Lot { get; set; }

        [BindProperty]
        public int TenantId { get; set; }

        public ParkingLotSelectModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public async Task OnGet(int tenantId, int floorId)
        {
            TenantId = tenantId;
            CarParkFloor floor = await _unitOfWork.CarParkFloor.LoadWithLots(floorId);
            Lot = floor.ParkingLots;
        }
    }
}
