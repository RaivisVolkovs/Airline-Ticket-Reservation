using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Services
{
    public class AdminService : IAdminService
    {

        private FlightDbRepository _flightRepository;
        private ITicketRepository _ticketRepository;



        public AdminService(FlightDbRepository flightRepository, ITicketRepository ticketRepository)
        {
            _flightRepository = flightRepository;
            _ticketRepository = ticketRepository;


        }

        public IEnumerable<ListTicketViewModel> ListTickets(Guid flightId)
        {
            try
            {
                IQueryable<Ticket> list = _ticketRepository.GetTickets().Where(x => x.FlightIdFK == flightId).OrderBy(x => x.FlightIdFK);

                var output = list.Select(p => new ListTicketViewModel
                {
                    Id = p.Id,
                    Row = p.Row,
                    Column = p.Column,
                    FlightIdFK = p.FlightIdFK,
                    Passport = p.Passport,
                    PricePaid = p.PricePaid,
                    Cancelled = p.Cancelled,
                }).ToList();

                return output;
            }
            catch (Exception ex)
            {
           
                throw new Exception($"Error in ListTickets service: {ex.Message}", ex);
            }
        }
    }
}
