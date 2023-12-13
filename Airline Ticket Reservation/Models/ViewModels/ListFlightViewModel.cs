namespace Airline_Ticket_Reservation.Models.ViewModels
{
    public class ListFlightViewModel
    {   
        public Guid Id { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string? CountryFrom { get; set; }
        public string? CountryTo { get; set; }
        public double WholesalePrice { get; set; }

        public double RetailPrice { get; set; }

        public bool IsActive { get; set; }
    }
    
}
