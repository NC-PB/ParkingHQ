using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using ParkingHQ.DataAccess.Repository.IRepository;
using ParkingHQ.Models;
using ParkingHQ.Utility;

namespace ParkingHQ.Web.Pages.Simulation
{
    public class NewTicketModel : PageModel
    {

        private readonly IUnitOfWork _unitOfWork;
        private ParkingHQ.Utility.ParkingUtility _parkingUtility;

        public NewTicketModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        CarPark carPark { get; set; }

        [BindProperty]
        public Ticket NewTicket { get; set; } = default!;

        [BindProperty]
        public CarParkFloor parkingLotFloor { get; set; }


        public async Task OnGet(int id)
        {
            _parkingUtility = new Utility.ParkingUtility(_unitOfWork);
            EntryExitUtility _entryExitUtility = new EntryExitUtility(_unitOfWork);



            NewTicket = await _parkingUtility.GetNewTicket(id);
            parkingLotFloor = _unitOfWork.CarParkFloor.LoadByParkingLotId(NewTicket.ParkingLot.Id).Result;



            await _entryExitUtility.AddVisitorEntry(NewTicket.ParkingLot.Id);
        }
    }
}
