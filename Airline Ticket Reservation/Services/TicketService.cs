using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

namespace Airline_Ticket_Reservation.Services
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;
        private IFlightRepository _flightRepository;


        public TicketService(ITicketRepository ticketRepository, IFlightRepository flightRepository)
        {
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
        }


        public void BookTicket(BookTicketViewModel bookingDetails)
        {
            var isSeatBooked = _ticketRepository.IsSeatBooked(bookingDetails.FlightIdFK, bookingDetails.Row, bookingDetails.Column);
            if (isSeatBooked)
            {
                throw new InvalidOperationException("The seat is already booked or was canceled.");
            }

            var flightDetails = _flightRepository.GetFlight(bookingDetails.FlightIdFK).SingleOrDefault();
            if (flightDetails == null || flightDetails.DepartureDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Invalid flight or the departure date is not in the future.");
            }

            _ticketRepository.Book(new Ticket()
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
            var flightDetails = _flightRepository.GetFlight(FlightIdFk).SingleOrDefault();
            var ticketDetails = _ticketRepository.GetTickets(FlightIdFk);

            BookTicketViewModel myModel = new BookTicketViewModel();
            myModel.Rows = flightDetails.Rows;
            myModel.Columns = flightDetails.Columns;
            myModel.OccupiedSeats = new List<(int, int)>();

            foreach (var ticket in ticketDetails)
            {
                myModel.OccupiedSeats.Add((ticket.Column, ticket.Row));
            }

            return myModel;
        }


    }
}
