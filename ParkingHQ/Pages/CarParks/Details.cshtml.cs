using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;


        [BindProperty]
        public CarPark currentCarpark { get; set; } 


        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public void OnGet(int id)
        {
            currentCarpark = _unitOfWork.CarPark.LoadAllComplete().Result.SingleOrDefault(x => x.Id == id);


        }
    }
}
