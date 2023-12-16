using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using Airline_Ticket_Reservation.Services;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Controllers
{
    [Authorize(Roles = "Admin")]
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
                TempData["error"] = "Tickets were not listed";
                return RedirectToAction("Index", "Home");
            }
        }



        [Route("Admin/ViewTicket/{ticketId}")]
      
        public IActionResult ViewTicket(Guid ticketId)
        {
            try
            {

                var viewTicket = _adminService.ViewTicket(ticketId);
                return View(viewTicket);
            }

            catch (Exception ex)
            {
                TempData["error"] = "Couldn't view a ticket";
                return RedirectToAction("Index", "Home");
            }

        }
    }
}

