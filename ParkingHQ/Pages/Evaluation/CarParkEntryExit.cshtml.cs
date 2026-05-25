using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.Evaluation
{
    public class CarParkEntryExitModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<EntryExit> EntryExitTenant { get; set; } = default!;

        [BindProperty]
        public IEnumerable<EntryExit> EntryExitVisitor { get; set; } = default!;

        [BindProperty]
        public int CarParkId { get; set; }

        [BindProperty]
        public DateTime DateFrom { get; set; }

        [BindProperty]
        public DateTime DateTo { get; set; }

        public CarParkEntryExitModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync(int id)
        {
            var entryExits = await _unitOfWork.EntryExit.GetEntryExitByCarParkId(id);
            EntryExitVisitor = entryExits.Where(x => x.PermanentTenant == null);
            EntryExitTenant = entryExits.Where(x => x.PermanentTenant != null);

            CarParkId = id;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        public async Task<ActionResult> OnPostFilterAsync(int CarparkId)
        {
            var entryExits = await _unitOfWork.EntryExit.GetEntryExitByCarParkId(CarparkId);

            EntryExitVisitor = entryExits
                .Where(x => x.PermanentTenant == null
                    && x.DateAndTime.Date >= DateFrom.Date
                    && x.DateAndTime.Date <= DateTo.Date)
                .ToList();
            EntryExitTenant = entryExits
                .Where(x => x.PermanentTenant != null
                    && x.DateAndTime.Date >= DateFrom.Date
                    && x.DateAndTime.Date <= DateTo.Date)
                .ToList();

            CarParkId = CarparkId;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            return Page();
        }

        public async Task<ActionResult> OnPostResetAsync(int CarparkId)
        {
            var entryExits = await _unitOfWork.EntryExit.GetEntryExitByCarParkId(CarparkId);
            EntryExitVisitor = entryExits.Where(x => x.PermanentTenant == null);
            EntryExitTenant = entryExits.Where(x => x.PermanentTenant != null);

            CarParkId = CarparkId;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            return Page();
        }

        public async Task<ActionResult> OnPostDownloadFileAsync(int CarparkId)
        {
            var entryExits = await _unitOfWork.EntryExit.GetEntryExitByCarParkId(CarparkId);
            string csv = ListToCSV(entryExits);
            byte[] bytes = Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/plain", "file.txt");
        }

        private string ListToCSV<T>(IEnumerable<T> list)
        {
            var sb = new StringBuilder();
            var props = typeof(T).GetProperties();
            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));

            foreach (var element in list)
            {
                sb.AppendLine(string.Join(",", props.Select(p => p.GetValue(element, null))));
            }

            return sb.ToString();
        }
    }
}
