﻿using DataAccess.DataContexts;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketDbRepository : ITicketRepository
    {
        private AirlineDbContext _airlineDbContext;

        public TicketDbRepository(AirlineDbContext airlineDbContext)
        {

            _airlineDbContext = airlineDbContext;

        }

        public void Book(Ticket ticket)
        {
            bool isSeatBooked = _airlineDbContext.Tickets.Any(t =>
                t.Row == ticket.Row &&
                t.Column == ticket.Column &&
                t.FlightIdFK == ticket.FlightIdFK &&
                !t.Cancelled);

            if (isSeatBooked)
            {
                throw new InvalidOperationException("The seat is already booked.");
            }

            // Proceed with booking if the seat is available
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


        public IQueryable<Ticket> GetTickets()
        {
            return _airlineDbContext.Tickets;
        }

        public Ticket? GetTicket(Guid Id)
        {
            return _airlineDbContext.Tickets.SingleOrDefault(x => x.Id == Id);
        }

        public int GetActiveTicketsCount(Guid Id)
        {
            return _airlineDbContext.Tickets.Count(x => x.FlightIdFK == Id && x.Cancelled == false);
        }

        public bool IsSeatBooked(Guid flightId, int row, int column)
        {
            return _airlineDbContext.Tickets.Any(ticket =>
                ticket.FlightIdFK == flightId &&
                ticket.Row == row &&
                ticket.Column == column &&
                !ticket.Cancelled);
        }
    }
}
