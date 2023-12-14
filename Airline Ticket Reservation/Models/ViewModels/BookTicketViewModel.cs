using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airline_Ticket_Reservation.Models.ViewModels
{
    public class BookTicketViewModel
    {



        public List<Ticket> tickets { get; set; }

        [Required(ErrorMessage = "Seating was not selected!")]
        public int Rows { get; set; }

        [Required(ErrorMessage = "Seating was not selected!")]
        public int Columns { get; set; }

        public Guid FlightIdFK { get; set; }


        [Required(ErrorMessage = "Passport photo is required!")]
        public IFormFile Passport { get; set; }



        [Required(ErrorMessage = "Passport number is required.")]
        [Display(Name = "Passport Number")]
        public string? PassportNo { get; set; }


        public List<(int, int)> OccupiedSeats { get; set; }

        public string[] SelectedSeats { get; set; }


    }
}
