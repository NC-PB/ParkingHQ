using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;
using System.Collections.Immutable;

namespace ParkingHQ.Web.Pages.Simulation.Tenant
{
    public class TenantExitModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private TenantUtility _tenantUtility;
        private TransactionUtility _transactionUtility;

        [BindProperty]
        public int Pin { get; set; }

        [BindProperty]
        public string Message { get; set; }

        [BindProperty]
        public int CarParkId { get; set; }

        public TenantExitModel(IUnitOfWork unitOfWork)
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

            if (_tenantUtility.AuthenticateTenant(Pin, CarParkId, true))
            {
                Message = "Exit allowed";

                // Data
                //
                PermanentTenantParkingLot tPermTen = _unitOfWork.PermanentTenantParkingLot.GetWithPropertysByPin(Pin);
                tPermTen.ParkingLot.IsOccupied = false;
                _unitOfWork.PermanentTenantParkingLot.Update(tPermTen);
                await _unitOfWork.Save();


                //Transaction
                //

                EntryExitUtility _entryExitUtility = new EntryExitUtility(_unitOfWork);
                await _entryExitUtility.AddTenantExit(tPermTen.ParkingLot.Id);



            }
            else
            {
                Message = "Exit denied";
            }

          return Page();
            

        }



    }
}
