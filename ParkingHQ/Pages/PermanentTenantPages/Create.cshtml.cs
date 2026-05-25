using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;


        [BindProperty]
        public PermanentTenant PermanentTenant { get; set; } = default!;
        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || PermanentTenant == null)
            {
                return Page();
            }

            await _unitOfWork.PermanentTenant.Add(PermanentTenant);
            await _unitOfWork.Save();

            return RedirectToPage("Index");
        }


    }
}
