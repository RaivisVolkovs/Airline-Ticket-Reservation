using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using Airline_Ticket_Reservation.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Controllers
{
    public class AdminController : Controller
    {
        private IFlightsService _flightRepository;
        private ITicketRepository _ticketRepository;
        private IAdminService _adminService;

        public AdminController(IFlightsService flightRepository, ITicketRepository ticketRepository, IAdminService adminService)
        {
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ListFlights()
        {
            var flights = _flightRepository.GetCurrentAvailableFlights();
            return View(flights);
        }

        public IActionResult ListTickets(Guid id)
        {
            try
            {
                var tickets = _adminService.ListTickets(id);
                return View(tickets);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ViewTicket(Guid ticketId)
        {

            return View();

        }
    }
}

