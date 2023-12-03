using DataAccess.DataContexts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketDbRepository
    {
        private AirlineDbContext _airlineDbContext;

        public TicketDbRepository(AirlineDbContext airlineDbContext) {
        
            _airlineDbContext = airlineDbContext;
        
        }

        public void Book(Ticket ticket)
        {
            _airlineDbContext.Tickets.Add(ticket);
            _airlineDbContext.SaveChanges();
        }

        public void Cancel(Guid Id)
        {
            var ticket = _airlineDbContext.Tickets.Find(Id);

            if (ticket != null)
            {
                ticket.Cancelled = true;
                _airlineDbContext.SaveChanges();
            }
        }


        public IQueryable <Ticket> GetTickets()
        {
            return _airlineDbContext.Tickets;
        }

        public Ticket? GetTicket(Guid Id)
        {
            return GetTickets().SingleOrDefault(x=> x.Id == Id);
        }


    }
}
