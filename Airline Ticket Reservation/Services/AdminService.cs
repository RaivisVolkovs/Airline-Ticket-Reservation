using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;

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
                    PassportNo = p.PassportNo,
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

        public ListTicketViewModel ViewTicket(Guid flightId)
        {

            var ticketDetails = _ticketRepository.GetTicket(flightId);

            if (ticketDetails == null)
            {
                return null;
            }

            var output = new ListTicketViewModel()
            {
                Id = ticketDetails.Id,
                Row = ticketDetails.Row,
                Column = ticketDetails.Column,
                FlightIdFK = ticketDetails.FlightIdFK,
                PassportNo = ticketDetails.PassportNo,
                Passport = ticketDetails.Passport,
                PricePaid = ticketDetails.PricePaid,
                Cancelled = ticketDetails.Cancelled,
            };

            return output;

        }

    }
}
