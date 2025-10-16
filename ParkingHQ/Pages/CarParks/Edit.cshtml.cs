using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.CarParks
{
    public class EditModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;



        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public CarPark CarPark { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _unitOfWork.CarPark.GetAll() == null)
            {
                return NotFound();
            }

            var carpark =  await _unitOfWork.CarPark.GetFirstOrDefault(m => m.Id == id);
            if (carpark == null)
            {
                return NotFound();
            }
            CarPark = carpark;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _unitOfWork.CarPark.Update(CarPark);
            _unitOfWork.Save();





            return RedirectToPage("./Index");
        }


    }
}
