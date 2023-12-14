using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.DataContexts;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Airline_Ticket_Reservation.Services
{
    public class TicketService : ITicketService
    {
        private ITicketRepository _ticketRepository;
        private IFlightRepository _flightRepository;
        private System.Security.Claims.ClaimsPrincipal? _currentUser;
        private IUserRepository _userDbRepository;

        public TicketService(ITicketRepository ticketRepository, IFlightRepository flightRepository, IHttpContextAccessor httpContextAccessor, IUserRepository userDbRepository)
        {
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
             _currentUser = httpContextAccessor.HttpContext?.User; //dabon lietotaju
            _userDbRepository = userDbRepository;
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
                            PricePaid = flightInfo.WholesalePrice * flightInfo.CommissionRate,
                            PassportNo=bookingDetails.PassportNo
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

                if(_currentUser.Identity.IsAuthenticated)
                {
                    string userId = _currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
                    var currentCustomUser = _userDbRepository.GetCurrentCustomUser(userId);  

                    myModel.PassportNo = currentCustomUser.PassportNo;
                }

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








        //public List<ListTicketViewModel> TicketHistory(string passportNo)
        //{
        //    var user = _userManager.FindByPassportNoAsync(passportNo).Result;

        //    if (user != null)
        //    {
        //        var userTickets = _ticketRepository.GetTickets(user.Id);

        //        var ticketViewModels = userTickets.Select(ticket => new ListTicketViewModel
        //        {
        //            FlightIdFK = ticket.FlightIdFK,
        //            Row = ticket.Row,
        //            Column = ticket.Column,
        //            PricePaid = ticket.PricePaid
        //        }).ToList();

        //        return ticketViewModels;
        //    }

        //    return null; 
        //}
    }

}

