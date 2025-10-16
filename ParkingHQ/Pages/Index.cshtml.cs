using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<CarPark> CarParks { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public void OnGet()
        {
            CarParks = _unitOfWork.CarPark.LoadAllComplete().Result;

        }
    }
}