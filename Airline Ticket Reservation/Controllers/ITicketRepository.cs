using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Controllers
{
    public class ITicketRepository : Controller
    {
        private TicketDbRepository _ticketRepository;
        private FlightDbRepository _flightRepository;


        public ITicketRepository(TicketDbRepository ticketRepository, FlightDbRepository flightRepository)
        {
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
        }

        public IActionResult Index()
        {
            try
            {
                IQueryable<Flight> list = _flightRepository.GetFlights().OrderBy(x => x.CountryFrom);

                var output = from p in list
                             select new ListFlightViewModel()
                             {
                                 Id = p.Id,
                                 DepartureDate = p.DepartureDate,    //izveidot View lapu, paastities no video 
                                 ArrivalDate = p.ArrivalDate,
                                 CountryFrom = p.CountryFrom,
                                 CountryTo = p.CountryTo,
                                 WholesalePrice = p.WholesalePrice

                             };
                return View(output);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
