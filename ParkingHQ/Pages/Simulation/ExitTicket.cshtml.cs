using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation
{
    public class ExitTicketModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private ParkingHQ.Utility.ParkingUtility _parkingUtility;

        public ExitTicketModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [BindProperty]
        public Ticket Ticket { get; set; } = default!;


        [BindProperty]
        public DateTime DateTimeExit { get; set; }

        [BindProperty]
        public decimal Tariff { get; set; }

        public void OnGet()
        {
            Tariff = 0;
            DateTimeExit= DateTime.Now;

        }

        public async Task<IActionResult> OnPostAsync()
        {
            _parkingUtility = new Utility.ParkingUtility(_unitOfWork);
            EntryExitUtility _entryExitUtility = new EntryExitUtility(_unitOfWork);
            TransactionUtility transactionUtility = new TransactionUtility(_unitOfWork);

            Ticket = _unitOfWork.Ticket.GetTicketByTicketId(Ticket.TicketId);


            //Calculate the tariff to pay and update the ticket
            //
            Ticket.ExitTime = DateTimeExit;
            Tariff = _parkingUtility.CalcTariffs(Ticket);
            Ticket.Price = Tariff;
            Ticket.ParkingLot.IsOccupied = false;
            _unitOfWork.Ticket.Update(Ticket);
            await _unitOfWork.Save();



            await _entryExitUtility.AddVisitorExit(Ticket.ParkingLot.Id);

            await transactionUtility.TicketPay(Ticket);


            return Page();

            //return RedirectToPage("./Index");
        }

    }
}
