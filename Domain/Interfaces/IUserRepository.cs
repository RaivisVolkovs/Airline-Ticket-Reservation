using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {

        public CustomUser? GetCurrentCustomUser(string userId);

        public IQueryable<Ticket> GetTicketHistory(string userId);
    }
}
