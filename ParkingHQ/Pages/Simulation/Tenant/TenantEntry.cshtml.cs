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
        private readonly TenantUtility _tenantUtility;
        private readonly EntryExitUtility _entryExitUtility;

        [BindProperty]
        public int Pin { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public int CarParkId { get; set; }

        public TenantEntryModel(
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
            if (await _tenantUtility.AuthenticateTenantAsync(Pin, CarParkId))
            {
                Message = "Entry allowed";
                PermanentTenantParkingLot tenantLot = await _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPinAsync(Pin);
                tenantLot.ParkingLot.IsOccupied = true;

                await _entryExitUtility.AddTenantEntry(tenantLot.ParkingLot.Id);

                _unitOfWork.ParkingLot.Update(tenantLot.ParkingLot);
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
