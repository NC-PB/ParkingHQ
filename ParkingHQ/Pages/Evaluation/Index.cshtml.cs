using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.Evaluation
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        [BindProperty]
        public IEnumerable<CarPark> CarParks { get; set; }


        public void OnGet()
        {
            CarParks = _unitOfWork.CarPark.GetAll();


        }
    }
}
