using Airline_Ticket_Reservation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Airline_Ticket_Reservation.Interfaces
{
    public interface IAdminService
    {

        public IEnumerable<ListTicketViewModel> ListTickets(Guid flightId);
    }
}
