using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using Airline_Ticket_Reservation.Services;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Airline_Ticket_Reservation.Controllers
{
    public class TicketController : Controller
    {
        private ITicketService _ticketService;
        private IFlightsService _flightsService;


        public TicketController(ITicketService ticketRepository, IFlightsService flightRepository)
        {
            _ticketService = ticketRepository;
            _flightsService = flightRepository;
        }

        public IActionResult Index()
        {
            try
            {

                return View(_flightsService.GetCurrentAvailableFlights());
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Route("Ticket/BookTicket/{FlightIdFk}")]
        public IActionResult BookTicket(Guid FlightIdFk)
        {
            return View(_ticketService.BookingDetails(FlightIdFk));
            
        }

        [HttpPost]
        public IActionResult BookTicket(BookTicketViewModel bookTicketViewModel, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                
                _ticketService.BookTicket(bookTicketViewModel, host);
                TempData["message"] = "Ticket added";
                return RedirectToAction("Index", "Ticket");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Ticket");
            }
        }

    }
}

