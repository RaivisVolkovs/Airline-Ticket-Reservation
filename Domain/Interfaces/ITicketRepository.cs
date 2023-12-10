using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITicketRepository
    {

        public void Book(Ticket ticket);

        public void Cancel(Guid Id);

        public IQueryable<Ticket> GetTickets();

        public Ticket? GetTicket(Guid Id);

        public int GetActiveTicketsCount(Guid Id);




    }
}
