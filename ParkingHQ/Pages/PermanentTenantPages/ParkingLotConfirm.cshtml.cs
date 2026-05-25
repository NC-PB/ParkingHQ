using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class ParkingLotConfirmModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TenantUtility _tenantUtility;

        [BindProperty]
        public PermanentTenant Tenant { get; set; }

        [BindProperty]
        public ParkingLot Lot { get; set; }

        [BindProperty]
        public CarPark SelectedCarPark { get; set; }

        [BindProperty]
        public CarParkFloor SelectedCarParkFloor { get; set; }

        public ParkingLotConfirmModel(IUnitOfWork unitOfWork, TenantUtility tenantUtility)
        {
            _unitOfWork = unitOfWork;
            _tenantUtility = tenantUtility;
        }

        public async Task OnGet(int tenantid, int lotid)
        {
            Tenant = await _unitOfWork.PermanentTenant.GetFirstOrDefault(u => u.Id == tenantid);
            Lot = await _unitOfWork.ParkingLot.GetFirstOrDefault(u => u.Id == lotid);
            SelectedCarPark = await _unitOfWork.CarPark.LoadByParkingLot(lotid);
            SelectedCarParkFloor = await _unitOfWork.CarParkFloor.LoadByParkingLotId(lotid);
        }

        public async Task<IActionResult> OnPost()
        {
            var tenantFromDb = await _unitOfWork.PermanentTenant.GetFirstOrDefault(u => u.Id == Tenant.Id);
            var lotFromDb = await _unitOfWork.ParkingLot.GetFirstOrDefault(u => u.Id == Lot.Id);

            lotFromDb.IsPermanentTenant = true;
            _unitOfWork.ParkingLot.Update(lotFromDb);
            await _unitOfWork.Save();

            PermanentTenantParkingLot tenantParkingLot = new PermanentTenantParkingLot
            {
                ParkingLot = lotFromDb,
                Pin = _tenantUtility.GeneratePermanentTenantPin()
            };

            tenantFromDb.PermanentTenantParkingLots.Add(tenantParkingLot);
            _unitOfWork.PermanentTenant.Update(tenantFromDb);
            await _unitOfWork.Save();

            return RedirectToPage("CarParkSelect", new { id = Tenant.Id });
        }
    }
}
