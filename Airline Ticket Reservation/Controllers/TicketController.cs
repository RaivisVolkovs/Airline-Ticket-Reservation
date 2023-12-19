using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using Airline_Ticket_Reservation.Services;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

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
            catch (Exception)
            {
                TempData["error"] = "Wasn't able to List the flights";
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
            catch (Exception)
            {
                TempData["error"] = "The ticket was not added";
                return RedirectToAction("Index", "Ticket");
            }
        }


        [HttpGet]
        [Route("Ticket/GetTicketHistory/{userId}")]
        public IActionResult GetTicketHistory(string userId)
        {
            
            try
            {
                var ticketHistory = _ticketService.GetTicketHistory(userId);
                return View(ticketHistory);
            }
            catch (Exception)
            {
                TempData["error"] = "Cant't access the Tickey History page";
                return RedirectToAction("Index", "Ticket");
            }
        }

        [Route("/TicketHistoryDetails/{ticketId}")]

        public IActionResult TicketHistoryDetails(Guid ticketId)
        {
            try
            {

                var viewTicket = _ticketService.TicketHistoryDetails(ticketId);
                return View(viewTicket);
            }

            catch (Exception ex)
            {
                TempData["error"] = "Couldn't view a ticket";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult ToggleCancel(Guid Id)
        {
            _ticketService.ToggleCancel(Id);
            return RedirectToAction("TicketHistoryDetails", "Ticket", new { ticketId = Id });
        }
    }
}

