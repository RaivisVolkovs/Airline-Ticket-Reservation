using Airline_Ticket_Reservation.Interfaces;
using Airline_Ticket_Reservation.Models.ViewModels;
using DataAccess.DataContexts;
using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web.Mvc;

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

                if (bookingDetails.Passport != null)
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
                    
                }

                if(bookingDetails.SelectedSeats == null) { 
                    throw new InvalidOperationException("No seat were added");
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
                            PassportNo = bookingDetails.PassportNo
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
            var ticketDetails = _ticketRepository.GetTickets().Where(t => t.FlightIdFK == FlightIdFk);

            if (flightDetails != null && ticketDetails != null)
            {
                BookTicketViewModel myModel = new BookTicketViewModel();
                myModel.Rows = flightDetails.Rows;
                myModel.Columns = flightDetails.Columns;
                myModel.OccupiedSeats = new List<(int, int)>();

                if (_currentUser.Identity.IsAuthenticated)
                {
                    string userId = _currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
                    var currentCustomUser = _userDbRepository.GetCurrentCustomUser(userId);

                    myModel.PassportNo = currentCustomUser.PassportNo;
                }

                foreach (var ticket in ticketDetails)
                {
                    if(ticket.Cancelled != true)
                    {
                        myModel.OccupiedSeats.Add((ticket.Column, ticket.Row));
                    }
                    
                }
                return myModel;
            }
            else
            {
                throw new InvalidOperationException("Invalid flight or ticket details");
            }
        }

        public IEnumerable<ListTicketViewModel> GetTicketHistory(string userId)
        {
            try
            {
                var user = _userDbRepository.GetCurrentCustomUser(_currentUser.FindFirstValue(ClaimTypes.NameIdentifier));

                if (user != null)
                {
                    IQueryable<Ticket> list = _ticketRepository.GetTickets().Where(x => x.PassportNo == user.PassportNo).OrderBy(x => x.Id); 

                    var output = list.Select(p => new ListTicketViewModel()
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
                }else
                {
                    throw new InvalidOperationException("Invalid flight or ticket details");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in TicketHistory service: {ex.Message}", ex);
            }



        }
        public ListTicketViewModel TicketHistoryDetails(Guid flightId)
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

        public void ToggleCancel(Guid Id)
        {
            _ticketRepository.Cancel(Id);
        }
    }
}

