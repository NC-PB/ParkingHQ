using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation
{
    public class NewTicketModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ParkingUtility _parkingUtility;
        private readonly EntryExitUtility _entryExitUtility;

        public NewTicketModel(
            IUnitOfWork unitOfWork,
            ParkingUtility parkingUtility,
            EntryExitUtility entryExitUtility)
        {
            _unitOfWork = unitOfWork;
            _parkingUtility = parkingUtility;
            _entryExitUtility = entryExitUtility;
        }

        [BindProperty]
        public Ticket NewTicket { get; set; } = default!;

        [BindProperty]
        public CarParkFloor ParkingLotFloor { get; set; }

        public async Task OnGetAsync(int id)
        {
            NewTicket = await _parkingUtility.GetNewTicket(id);
            ParkingLotFloor = await _unitOfWork.CarParkFloor.LoadByParkingLotId(NewTicket.ParkingLot.Id);

            await _entryExitUtility.AddVisitorEntry(NewTicket.ParkingLot.Id);
        }
    }
}
