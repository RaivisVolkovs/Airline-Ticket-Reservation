using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketFileRepository: ITicketRepository
    {

        string filePath;

        public TicketFileRepository(string pathToTicketsFile)
        {

            filePath = pathToTicketsFile;

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

            var myList = GetTickets().ToList();

            myList.Add(ticket);

            string jsonString = JsonSerializer.Serialize(myList);

            System.IO.File.WriteAllText(filePath, jsonString);
        }


        public Ticket? GetTicket(Guid id)
        {

            return GetTickets().SingleOrDefault(x => x.Id == id);
        }



        public IQueryable<Ticket> GetTickets()
        {
            //StreamReader, System.IO.File.OpenText

            string allText = System.IO.File.ReadAllText(filePath);

            //StreamReader sr = new StreamReader(filePath);
            //allText= sr.ReadToEnd();  
            if (allText == "")
            {
                return new List<Ticket>().AsQueryable();
            }
            else
            {
                //note: next line will convert from normal text into json-formatted-object
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

        public void Cancel(Guid id)
        {
            List<Ticket> tickets = GetTickets().ToList();

            // Find the index of the ticket with the specified ID
            int index = tickets.FindIndex(t => t.Id == id);

            // If the ticket with the specified ID is found, mark it as canceled
            if (index != -1)
            {
                tickets[index].Cancelled = true;

                // Serialize the updated list and write it back to the file
                string jsonString = JsonSerializer.Serialize(tickets);
                System.IO.File.WriteAllText(filePath, jsonString);
            }
        }

        public IQueryable<Ticket> GetActiveTickets(Guid Id)
        {
            throw new NotImplementedException();
        }

        public int GetActiveTicketsCount(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
