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

            var flightDetails = _flightRepository.GetFlight(bookingDetails.FlightIdFK);
            if (flightDetails == null || flightDetails.DepartureDate <= DateTime.Now)
            {
                throw new InvalidOperationException("Invalid flight or the departure date is not in the future.");
            }

            foreach (string seat in bookingDetails.SelectedSeats)
            {
                int[] seatInfo = seat.Split(';').Select(n => Convert.ToInt32(n)).ToArray();
                Flight? flightInfo = _flightRepository.GetFlight(bookingDetails.FlightIdFK);
                if (flightInfo != null)
                {
                    _ticketRepository.Book(new Ticket()
                    {
                        Row = seatInfo[0],
                        Column = seatInfo[1],
                        FlightIdFK = bookingDetails.FlightIdFK,
                        Passport = bookingDetails.Passport,
                        PricePaid = flightInfo.WholesalePrice * flightInfo.CommissionRate
                    });
                }
                else
                {
                    throw new InvalidOperationException("Missing flight prices.");
                }

            }
        }


        public BookTicketViewModel BookingDetails(Guid FlightIdFk)
        {
            var flightDetails = _flightRepository.GetFlight(FlightIdFk);
            var ticketDetails = _ticketRepository.GetTickets();

            if (flightDetails != null && ticketDetails != null)
            {
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
            else
            {
                throw new InvalidOperationException("Invalid flight or ticket details");
            }
        }
    }
}
