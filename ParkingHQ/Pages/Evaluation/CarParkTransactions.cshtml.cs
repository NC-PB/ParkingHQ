using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParkingHQ.DataAccess;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;

namespace ParkingHQ.Web.Pages.Evaluation
{
    public class CarParkTransactionsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public IEnumerable<Transaction> EntryExitTenant { get; set; } = default!;

        [BindProperty]
        public IEnumerable<Transaction> EntryExitVisitor { get; set; } = default!;


        [BindProperty]
        public int CarParkId { get; set; }

        [BindProperty]
        public DateTime DateFrom { get; set; }

        [BindProperty]
        public DateTime DateTo { get; set; }

        public CarParkTransactionsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }


        public async Task OnGetAsync(int id)
        {
            var entryExit = await _unitOfWork.CarPark.GetTransactions(id);
            EntryExitVisitor = entryExit.Transactions.Where(x => x.PermanentTenant == null);
            EntryExitTenant = entryExit.Transactions.Where(x => x.PermanentTenant != null);

            CarParkId = id;
            DateFrom = DateTime.Now;
            DateTo= DateTime.Now;
        }



        public ActionResult OnPostFilter(int CarparkId)
        {
            var entryExit = _unitOfWork.CarPark.GetTransactions(CarparkId).Result;


            //Filter by Date
            //
            EntryExitVisitor = entryExit.Transactions.Where(x => x.PermanentTenant == null && (x.TransactionDate.Date >= DateFrom.Date && x.TransactionDate.Date <= DateTo.Date)).ToList();
            EntryExitTenant = entryExit.Transactions.Where(x => x.PermanentTenant != null && (x.TransactionDate.Date >= DateFrom.Date && x.TransactionDate.Date <= DateTo.Date)).ToList();

            CarParkId = CarparkId;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            return Page();
        }


        public ActionResult OnPostReset(int CarparkId)
        {
            var entryExit = _unitOfWork.CarPark.GetTransactions(CarparkId).Result;
            EntryExitVisitor = entryExit.Transactions.Where(x => x.PermanentTenant == null);
            EntryExitTenant = entryExit.Transactions.Where(x => x.PermanentTenant == null);

            CarParkId = CarparkId;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            return Page();
        }



        public ActionResult OnPostDownloadFile(int CarparkId)
        {
            var entryExit =  _unitOfWork.EntryExit.GetEntryExitByCarParkId(CarparkId).Result;

            //EntryExitVisitor = entryExit.Where(x => x.PermanentTenant == null);
            //EntryExitTenant = entryExit.Where(x => x.PermanentTenant != null);

            string list = ListToCSV(entryExit);

            byte[] bytes = Encoding.UTF8.GetBytes(list);
            return File(bytes, "text/plain", "file.txt");
        }




        private string CreateCSV(IEnumerable<EntryExit> list)
        {
            StringBuilder sList = new StringBuilder();

            sList.Append("DateTime;Type;Tenant;");
            sList.Append(Environment.NewLine);

            string separator = ";";

            foreach (var element in list)
            {
                sList.Append(element.DateAndTime);
                sList.Append(separator);
                sList.Append(element.EntryExitType.ToString());
                sList.Append(separator);
                if (element.PermanentTenant != null)
                {
                    sList.Append(element.PermanentTenant.FirstName + " " + element.PermanentTenant.LastName);
                }
                else
                {
                    sList.Append(" ");
                }
                sList.Append(separator);
                sList.Append(element.Parkinglot.ParkinLotNumber);
                sList.Append(Environment.NewLine);
            }

            return sList.ToString();
        }






        private string ListToCSV<T>(IEnumerable<T> list)
        {
            StringBuilder sList = new StringBuilder();

            Type type = typeof(T);
            var props = type.GetProperties();
            sList.Append(string.Join(",", props.Select(p => p.Name)));
            sList.Append(Environment.NewLine);

            foreach (var element in list)
            {
                sList.Append(string.Join(",", props.Select(p => p.GetValue(element, null))));
                sList.Append(Environment.NewLine);
            }

            return sList.ToString();
        }


    }
}
