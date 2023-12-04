using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Controllers
{
    public class TicketController : Controller
    {
        private ITicketRepository _ticketRepository;
        private IFlightsService _flightsService;


        public TicketController(ITicketRepository ticketRepository, IFlightsService flightRepository)
        {
            _ticketRepository = ticketRepository;
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
    }
}
