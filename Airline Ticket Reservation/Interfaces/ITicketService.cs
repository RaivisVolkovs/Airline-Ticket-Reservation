﻿using Airline_Ticket_Reservation.Models.ViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Interfaces
{
    public interface ITicketService
    {
        public void BookTicket(BookTicketViewModel bookingDetails, IWebHostEnvironment hostEnvironment);
        public BookTicketViewModel BookingDetails(Guid FlightIdFk);

        public IEnumerable<ListTicketViewModel> GetTicketHistory(string userId);

        public ListTicketViewModel TicketHistoryDetails(Guid flightId);

        public void ToggleCancel(Guid Id);
    }
}
