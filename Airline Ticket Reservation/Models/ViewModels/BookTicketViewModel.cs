using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline_Ticket_Reservation.Models.ViewModels
{
    public class BookTicketViewModel
    {


        public List<Flight> flights { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public Guid FlightIdFK { get; set; }


        [Required(ErrorMessage = "Please upload a passport photo.")]
        [Display(Name = "Passport Photo")]
        public string Passport { get; set; }

        public int PricePaid { get; set; }

        public List<(int, int)> OccupiedSeats { get; set; }

        [Required(ErrorMessage = "It's required")]
        public int Row { get; set; }

        [Required(ErrorMessage = "It's required")]
        public int Column { get; set; }

    }
}
