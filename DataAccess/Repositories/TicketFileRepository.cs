using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml;

namespace DataAccess.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {
        string filePath;

        public TicketFileRepository(string pathToProductsFile)
        {
            filePath = pathToProductsFile;

            if (System.IO.File.Exists(filePath) == false)
            {
                using (var myFile = System.IO.File.Create(filePath))
                {
                    myFile.Close();
                }
            }
        }

        public void Book(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();

            var tickets = GetTickets().ToList();
            tickets.Add(ticket);

            string jsonString = JsonSerializer.Serialize(tickets);

            System.IO.File.WriteAllText(filePath, jsonString);
        }

        public void Cancel(Guid id)
        {
            var tickets = GetTickets().ToList();

            int index = tickets.FindIndex(t => t.Id == id);

            if (index != -1)
            {
                tickets.RemoveAt(index);

                string jsonString = JsonSerializer.Serialize(tickets);
                System.IO.File.WriteAllText(filePath, jsonString);
            }


        }


        public int GetActiveTicketsCount(Guid flightId)
        {
            return GetTickets().Count(x => x.FlightIdFK == flightId && x.Cancelled == false);
        }

        public Ticket? GetTicket(Guid id)
        {
            return GetTickets().FirstOrDefault(t => t.Id == id);
        }

        public IQueryable<Ticket> GetTickets()
        {
            string allText = System.IO.File.ReadAllText(filePath);

            if (allText == "")
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                try
                {
                    List<Ticket> tickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                    return tickets.AsQueryable();
                }
                catch
                {
                    return new List<Ticket>().AsQueryable();
                }
            }
        }

        public bool IsSeatBooked(Guid flightId, int row, int column)
        {
            return GetTickets().Any(t => t.FlightIdFK == flightId && t.Row == row && t.Column == column && !t.Cancelled);
        }
    }
}

