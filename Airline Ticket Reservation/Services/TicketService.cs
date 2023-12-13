using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Web.Mvc;

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


        public void BookTicket(BookTicketViewModel bookingDetails, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                string relativePath = "";

                if(bookingDetails.Passport != null)
                {
                    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(bookingDetails.Passport.FileName);

                    relativePath = "/images/" + newFileName;

                    string absolutePath = host.WebRootPath + "\\images\\" + newFileName;

                    using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                    {
                        bookingDetails.Passport.CopyTo(fs);
                        fs.Flush();
                    }
                }
              
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
                            Passport = relativePath,
                            PricePaid = flightInfo.WholesalePrice * flightInfo.CommissionRate
                        });
                    }
                }
            }
            catch
            {

                throw new InvalidOperationException("Invalid ticket");

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
