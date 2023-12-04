﻿using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;

namespace Airline_Ticket_Reservation.Services
{
    public class FlightService : IFlightsService
    {
        private FlightDbRepository _flightRepository;
        private ITicketRepository _ticketRepository;


        public FlightService( FlightDbRepository flightRepository, ITicketRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;

        }
        public IEnumerable<ListFlightViewModel> GetCurrentAvailableFlights()
        {
            IQueryable<Flight> list = _flightRepository.GetFlights().Where(x => x.DepartureDate > DateTime.Now);

            var output = from p in list
                         select new ListFlightViewModel()
                         {
                             Id = p.Id,
                             DepartureDate = p.DepartureDate,
                             ArrivalDate = p.ArrivalDate,
                             CountryFrom = p.CountryFrom,
                             CountryTo = p.CountryTo,
                             WholesalePrice = p.WholesalePrice,
                             IsActive = _ticketRepository.GetActiveTicketsCount(p.Id) < (p.Rows * p.Columns),

                         };
            return output;
        }
    }
}