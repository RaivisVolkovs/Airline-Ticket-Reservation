using Airline_Ticket_Reservation.Models.ViewModels;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline_Ticket_Reservation.Interfaces
{
    public interface IFlightsService
    {
        public IEnumerable<ListFlightViewModel> GetCurrentAvailableFlights();

    }
}
