using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks
{
    public class CreateModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public CarPark CarPark { get; set; }

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        { 
            await _unitOfWork.CarPark.Add(CarPark);
            await _unitOfWork.Save();

            
            return RedirectToPage("Index");
        }

        
    }
}
