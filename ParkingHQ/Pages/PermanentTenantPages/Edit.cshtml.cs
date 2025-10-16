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

namespace ParkingHQ.Web.Pages.PermanentTenantPages
{
    public class EditModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        public EditModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public PermanentTenant PermanentTenant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var permanenttenant =  await _context.PermanentTenants.FirstOrDefaultAsync(m => m.Id == id);
            var permanenttenant = await _unitOfWork.PermanentTenant.GetFirstOrDefault(m => m.Id == id);
            if (permanenttenant == null)
            {
                return NotFound();
            }
            PermanentTenant = permanenttenant;
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

            _unitOfWork.PermanentTenant.Update(PermanentTenant);
            await _unitOfWork.Save();
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!PermanentTenantExists(PermanentTenant.Id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return RedirectToPage("./Index");
        }

    }
}
