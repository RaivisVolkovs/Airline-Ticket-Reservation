namespace Airline_Ticket_Reservation.Models.ViewModels
{
    public class ListTicketViewModel
    {
        public Guid Id { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public Guid FlightIdFK { get; set; }

        public string Passport { get; set; }

        public double PricePaid { get; set; }

        public bool Cancelled { get; set; }
    }
}
