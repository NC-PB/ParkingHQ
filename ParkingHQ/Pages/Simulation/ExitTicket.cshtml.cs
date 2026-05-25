using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation
{
    public class ExitTicketModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ParkingUtility _parkingUtility;
        private readonly EntryExitUtility _entryExitUtility;
        private readonly TransactionUtility _transactionUtility;

        public ExitTicketModel(
            IUnitOfWork unitOfWork,
            ParkingUtility parkingUtility,
            EntryExitUtility entryExitUtility,
            TransactionUtility transactionUtility)
        {
            _unitOfWork = unitOfWork;
            _parkingUtility = parkingUtility;
            _entryExitUtility = entryExitUtility;
            _transactionUtility = transactionUtility;
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
            DateTimeExit = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Ticket = await _unitOfWork.Ticket.GetTicketByTicketIdAsync(Ticket.TicketId);

            Ticket.ExitTime = DateTimeExit;
            Tariff = await _parkingUtility.CalcTariffAsync(Ticket);
            Ticket.Price = Tariff;
            Ticket.ParkingLot.IsOccupied = false;
            _unitOfWork.Ticket.Update(Ticket);
            await _unitOfWork.Save();

            await _entryExitUtility.AddVisitorExit(Ticket.ParkingLot.Id);
            await _transactionUtility.TicketPay(Ticket);

            return Page();
        }
    }
}
