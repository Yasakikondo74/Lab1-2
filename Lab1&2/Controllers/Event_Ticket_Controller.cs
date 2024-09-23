using Lab1_2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Collections.Generic;

namespace Lab1_2.Controllers
{
    public class Event_Ticket_Controller : Controller
    {
        private readonly Event_Ticket_DbContext _db;

        public Event_Ticket_Controller(Event_Ticket_DbContext db)
        {
            _db = db;
        }
        public IActionResult List()
        {
            var data = _db.theEvents.ToList();
            var EventData = data.Select(Snapshot => new TheEvent
            {
                EventID = Snapshot.EventID,
                EventName = Snapshot.EventName,
                TotalTickets = Snapshot.Tickets.Count(t => t.TheEventId == Snapshot.EventID)}).ToList();
            return View(EventData);
        }
        public IActionResult Create()
        {
            return View();
        }
        private List<Ticket> GenerateRandomTickets(Guid eventId, int numberOfTickets)
        {
            var random = new Random();
            var tickets = new List<Ticket>();

            for (int i = 0; i < numberOfTickets; i++)
            {
                var ticket = new Ticket
                {
                    TicketName = GenerateRandomTicketName(random),
                    TheEventId = eventId
                };
                tickets.Add(ticket);
            }

            return tickets;
        }
        private string GenerateRandomTicketName(Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        public IActionResult Create(TheEvent theEvent, int numberOfTickets)
        {
            _db.theEvents.Add(theEvent);
            _db.SaveChanges();

            var tickets = GenerateRandomTickets(theEvent.EventID, numberOfTickets);
            foreach (var ticket in tickets)
            {
                _db.Tickets.Add(ticket);
            }
            _db.SaveChanges();
            return RedirectToAction("List");
        }
        [HttpPost]
        public IActionResult Edit(Guid id)
        {
            var Find = _db.theEvents.FirstOrDefault(x => x.EventID == id);
            _db.theEvents.Update(Find);
            _db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete(Guid id)
        {
            var Find = _db.theEvents.FirstOrDefault(x => x.EventID == id);
            var ticketsToDelete = _db.Tickets.Where(t => t.TheEventId == id).ToList();
            _db.Tickets.RemoveRange(ticketsToDelete);
            _db.theEvents.Remove(Find);
            _db.SaveChanges();
            return RedirectToAction("List");
        }
        public IActionResult Delete_Except(Guid id)
        {
            var eventToDelete = _db.theEvents.FirstOrDefault(x => x.EventID == id);
            var relatedTickets = _db.Tickets.Where(t => t.TheEventId == id).ToList();
            if (!relatedTickets.Any())
            {
                _db.theEvents.Remove(eventToDelete);
                _db.SaveChanges();
            }
            else
            {
                TempData["Warning"] = "Please delete all tickets associated with this event before deleting the event.";
            }
            return RedirectToAction("List");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
