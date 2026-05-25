using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class CarParkSelectModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TransactionUtility _transactionUtility;

        [BindProperty]
        public IEnumerable<CarPark> CarParks { get; set; }

        [BindProperty]
        public List<CarParkSelectHelper> CarParkSelectHelper { get; set; }

        public PermanentTenant PermanentTenant { get; set; }

        public CarParkSelectModel(IUnitOfWork unitOfWork, TransactionUtility transactionUtility)
        {
            _unitOfWork = unitOfWork;
            _transactionUtility = transactionUtility;
        }

        public async Task OnGet(int Id)
        {
            CarParks = _unitOfWork.CarPark.GetAll();
            PermanentTenant = await _unitOfWork.PermanentTenant.GetWithPropertiesById(Id);

            CarParkSelectHelper = new List<CarParkSelectHelper>();

            foreach (PermanentTenantParkingLot lot in PermanentTenant.PermanentTenantParkingLots)
            {
                CarParkSelectHelper cHelper = new CarParkSelectHelper
                {
                    sPermanentTenantParkingLotId = lot.Id,
                    sCarPark = await _unitOfWork.CarPark.LoadByParkingLot(lot.ParkingLot.Id),
                    sCarParkFloor = await _unitOfWork.CarParkFloor.LoadByParkingLotId(lot.ParkingLot.Id),
                    sParkingLots = lot.ParkingLot,
                    LastPayment = DateOnly.FromDateTime(lot.LastPayment.Date),
                    pin = lot.Pin
                };
                CarParkSelectHelper.Add(cHelper);
            }
        }

        public async Task<IActionResult> OnPostDelete(int id, int TenantId)
        {
            var lotEntry = await _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByIdAsync(id);
            var lot = await _unitOfWork.ParkingLot.GetFirstOrDefault(u => u.Id == lotEntry.ParkingLot.Id);
            lot.IsPermanentTenant = false;

            _unitOfWork.ParkingLot.Update(lot);
            _unitOfWork.PermanentTenantParkingLot.Remove(lotEntry);
            await _unitOfWork.Save();

            return RedirectToPage("CarParkSelect", new { id = TenantId });
        }

        public async Task<IActionResult> OnPostPay(int id, int TenantId)
        {
            var paymentUpdate = await _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByIdAsync(id);
            paymentUpdate.LastPayment = DateTime.Now;
            _unitOfWork.PermanentTenantParkingLot.Update(paymentUpdate);
            await _unitOfWork.Save();

            await _transactionUtility.TenantPay(TenantId, paymentUpdate.ParkingLot);

            return RedirectToPage("CarParkSelect", new { id = TenantId });
        }
    }

    public struct CarParkSelectHelper
    {
        public int sPermanentTenantParkingLotId { get; set; }
        public CarPark sCarPark { get; set; }
        public CarParkFloor sCarParkFloor { get; set; }
        public ParkingLot sParkingLots { get; set; }
        public int pin { get; set; }
        public DateOnly LastPayment { get; set; }
    }
}
