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

        [BindProperty]
        public IEnumerable<CarPark> CarParks { get; set; }


        [BindProperty]
        public List<CarParkSelectHelper> CarParkSelectHelper { get; set; }


        public PermanentTenant PermanentTenant { get; set; }
        public CarParkSelectModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }



        public async Task OnGet(int Id)
        {
            CarParks = _unitOfWork.CarPark.GetAll();
            PermanentTenant = await _unitOfWork.PermanentTenant.GetWithPropertiesById(Id);

            CarParkSelectHelper = new List<CarParkSelectHelper>();

            foreach (PermanentTenantParkingLot lot in PermanentTenant.PermanentTenantParkingLots)
            { 
                CarParkSelectHelper cHelper = new CarParkSelectHelper();

                cHelper.sPermanentTenantParkingLotId = lot.Id;
                cHelper.sCarPark = _unitOfWork.CarPark.LoadByParkingLot(lot.ParkingLot.Id).Result;
                cHelper.sCarParkFloor = _unitOfWork.CarParkFloor.LoadByParkingLotId(lot.ParkingLot.Id).Result;
                cHelper.sParkingLots = lot.ParkingLot;
                cHelper.LastPayment= DateOnly.FromDateTime(lot.LastPayment.Date);
                cHelper.pin = lot.Pin;
                CarParkSelectHelper.Add(cHelper);
            }

        }


        public async Task<IActionResult> OnPostDelete(int id, int TenantId)
        {
            var delete = _unitOfWork.PermanentTenantParkingLot.GetWithPropertysById(id);

            var lot = _unitOfWork.ParkingLot.GetFirstOrDefault(u => u.Id == delete.ParkingLot.Id).Result;
            lot.IsPermanentTenant = false;

            _unitOfWork.ParkingLot.Update(lot);

            _unitOfWork.PermanentTenantParkingLot.Remove(delete);
            await _unitOfWork.Save();
            return RedirectToPage("CarParkSelect", new { id = TenantId });
        }

        public async Task<IActionResult> OnPostPay(int id, int TenantId)
        {

            TransactionUtility _transactionUtility = new TransactionUtility(_unitOfWork);

            var paymentUpdate = _unitOfWork.PermanentTenantParkingLot.GetWithPropertysById(id);
            paymentUpdate.LastPayment = DateTime.Now;
            _unitOfWork.PermanentTenantParkingLot.Update(paymentUpdate);
            await _unitOfWork.Save();

            await _transactionUtility.TenantPay(TenantId, paymentUpdate.ParkingLot);

            return RedirectToPage("CarParkSelect", new { id = TenantId });
        }





    }

    public struct CarParkSelectHelper
    {
        public int  sPermanentTenantParkingLotId { get; set; }
        public CarPark sCarPark { get; set; }
        public CarParkFloor sCarParkFloor { get; set; }
        public ParkingLot sParkingLots { get; set; }
        public int pin { get; set; }
        public DateOnly LastPayment { get; set; }
    }


}
