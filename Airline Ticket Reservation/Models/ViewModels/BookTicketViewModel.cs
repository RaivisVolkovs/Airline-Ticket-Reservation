using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline_Ticket_Reservation.Models.ViewModels
{
    public class BookTicketViewModel
    {


        public List<Flight> flights { get; set; }

        public int Row { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public int Column { get; set; }

        public Guid FlightIdFK { get; set; }


        [Required(ErrorMessage = "Symbols should be 12 length")]
        public string Passport { get; set; }

        public int PricePaid { get; set; }

        public List<(int, int)> OccupiedSeats { get; set; }

    }
}
