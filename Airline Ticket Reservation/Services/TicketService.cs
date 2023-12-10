using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Airline_Ticket_Reservation.Services
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;


        public TicketService( ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;

        }


        public void BookTicket(BookTicketViewModel bookingDetails)
        {

            _ticketRepository.Book( new Ticket ()
                {
                Row = bookingDetails.Row,
                Column = bookingDetails.Column,
                FlightIdFK = bookingDetails.FlightIdFK,
                Passport = bookingDetails.Passport,
                PricePaid = bookingDetails.PricePaid,
            });
        }

        public BookTicketViewModel BookingDetails(Guid FlightIdFk) 
        {
            BookTicketViewModel myModel = new BookTicketViewModel();
            myModel.Rows = 20;
            myModel.Columns = 5;
            myModel.OccupiedSeats = new List<(int, int)>();
            myModel.OccupiedSeats.Add((1, 4));
            myModel.OccupiedSeats.Add((2, 7));

            return myModel;
        }

    }

}
