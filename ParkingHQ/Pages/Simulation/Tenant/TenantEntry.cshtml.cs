using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation.Tenant
{
    public class TenantEntryModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private TenantUtility _tenantUtility;

        [BindProperty]
        public int Pin { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public int CarParkId { get; set; }

        public TenantEntryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void OnGet(int Id)
        {
            Message = String.Empty;
            CarParkId= Id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _tenantUtility = new TenantUtility(_unitOfWork);

            if (_tenantUtility.AuthenticateTenant(Pin, CarParkId))
            {
                Message = "Entry allowed";
                PermanentTenantParkingLot tPermTen = _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPin(Pin);
                tPermTen.ParkingLot.IsOccupied = true;

                EntryExitUtility _entryExitUtility = new EntryExitUtility(_unitOfWork);
                await _entryExitUtility.AddTenantEntry(tPermTen.ParkingLot.Id);

                _unitOfWork.ParkingLot.Update(tPermTen.ParkingLot);
                await _unitOfWork.Save();

            }
            else
            {
                Message = "Entry denied";
            }

          return Page();
            

        }



    }
}
