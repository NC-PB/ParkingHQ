using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;
using System.Drawing;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class ParkingLotConfirmModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;



        [BindProperty]
        public PermanentTenant Tenant { get; set; }

        [BindProperty]
        public ParkingLot Lot { get; set; }

        [BindProperty]
        public CarPark SelectedCarPark { get; set; }

        [BindProperty]
        public CarParkFloor SelectedCarParkFloor { get; set; }

        public ParkingLotConfirmModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            //Load the ParkingLot and PermanentTenant from the database
            var TenantFromDb = await _unitOfWork.PermanentTenant.GetFirstOrDefault(u => u.Id == Tenant.Id);
            var LotFromDb = await _unitOfWork.ParkingLot.GetFirstOrDefault(u => u.Id == Lot.Id);

            //Update Status of the paking lot
            LotFromDb.IsPermanentTenant = true;
            _unitOfWork.ParkingLot.Update(LotFromDb);
            await _unitOfWork.Save();

            //Create a new PermanentTenantParkingLot object
            PermanentTenantParkingLot permanentTenantParkingLog = new PermanentTenantParkingLot();

            //Set the properties of the PermanentTenantParkingLot object
            permanentTenantParkingLog.ParkingLot = LotFromDb;

            //Generate a new PIN
            TenantUtility tenantUtiliy = new TenantUtility(_unitOfWork);
            permanentTenantParkingLog.Pin = tenantUtiliy.GeneratePermanentTenantPin();


            TenantFromDb.PermanentTenantParkingLots.Add(permanentTenantParkingLog);
            _unitOfWork.PermanentTenant.Update(TenantFromDb);
            _unitOfWork.Save();

            //var categoryFromDb = _db.Categorys.Find(Category.Id);
            //if (categoryFromDb != null)
            //{
            //    _db.Categorys.Remove(categoryFromDb);
            //    await _db.SaveChangesAsync();
            //    TempData["success"] = "Category has been deleted.";

            //    return RedirectToPage("Index");
            //}

            return RedirectToPage("CarParkSelect", new { id = Tenant.Id });

            //return Page();

        }



    }
}
