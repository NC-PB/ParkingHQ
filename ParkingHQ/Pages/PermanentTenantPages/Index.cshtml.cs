using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class IndexModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<PermanentTenant> PermanentTenants { get; set; }


        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            PermanentTenants = _unitOfWork.PermanentTenant.GetAllIncluding(x => x.PermanentTenantParkingLots).ToList();
        }


    }
}
