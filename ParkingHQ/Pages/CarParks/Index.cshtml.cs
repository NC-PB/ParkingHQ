using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks
{
    public class IndexModel : PageModel
    {
        
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<CarPark> CarParks { get; set; }


        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public void OnGet()
        {
            // Read all car parks and display them in a table
            CarParks = _unitOfWork.CarPark.GetAll();
        }
    }
}
