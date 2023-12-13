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


        public List<(int, int)> OccupiedSeats { get; set; }

        public string[] SelectedSeats { get; set; }


    }
}
