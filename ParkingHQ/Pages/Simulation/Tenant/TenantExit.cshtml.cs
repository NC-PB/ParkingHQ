using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation.Tenant
{
    public class TenantExitModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly TenantUtility _tenantUtility;
        private readonly EntryExitUtility _entryExitUtility;

        [BindProperty]
        public int Pin { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public int CarParkId { get; set; }

        public TenantExitModel(
            IUnitOfWork unitOfWork,
            TenantUtility tenantUtility,
            EntryExitUtility entryExitUtility)
        {
            _unitOfWork = unitOfWork;
            _tenantUtility = tenantUtility;
            _entryExitUtility = entryExitUtility;
        }

        public void OnGet(int Id)
        {
            Message = string.Empty;
            CarParkId = Id;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await _tenantUtility.AuthenticateTenantAsync(Pin, CarParkId, exit: true))
            {
                Message = "Exit allowed";

                PermanentTenantParkingLot tenantLot = await _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPinAsync(Pin);
                tenantLot.ParkingLot.IsOccupied = false;
                _unitOfWork.PermanentTenantParkingLot.Update(tenantLot);
                await _unitOfWork.Save();

                await _entryExitUtility.AddTenantExit(tenantLot.ParkingLot.Id);
            }
            else
            {
                Message = "Exit denied";
            }

            return Page();
        }
    }
}
