using DataAccess.DataContexts;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserDbRepository: IUserRepository
    {
        private ITicketRepository _ticketRepository;
        private AirlineDbContext _airlineDbContext;

        public UserDbRepository(ITicketRepository ticketRepository, AirlineDbContext airlineDbContext) 
        {
            _ticketRepository = ticketRepository;
            _airlineDbContext = airlineDbContext;
        }

        public CustomUser? GetCurrentCustomUser(string userId)
        {
            return _airlineDbContext.Users.SingleOrDefault(x => x.Id == userId);
        }

        public IQueryable<Ticket> GetTicketHistory(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
