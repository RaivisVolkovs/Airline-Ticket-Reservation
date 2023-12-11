using DataAccess.DataContexts;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FlightDbRepository : IFlightRepository
    {

        private AirlineDbContext _airlineDbContext;
        public FlightDbRepository(AirlineDbContext airlineDbContext)
        {

            _airlineDbContext = airlineDbContext;

        }

        public IQueryable<Flight> GetFlights()
        {
            return _airlineDbContext.Flights;
        }

        public IQueryable<Flight> GetFlight(Guid Id)
        {
            return _airlineDbContext.Flights.Where(flight => flight.Id == Id);
        }

    }
}
