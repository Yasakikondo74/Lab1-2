using Microsoft.EntityFrameworkCore;

namespace Lab1_2.Models
{
    public class Event_Ticket_DbContext : DbContext
    {
        public Event_Ticket_DbContext(DbContextOptions<Event_Ticket_DbContext> options) : base(options)
        {
        }
        public Event_Ticket_DbContext()
        {
        }
        public DbSet<TheEvent> theEvents { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TheEvent>().HasKey(e => e.EventID);
            base.OnModelCreating(modelBuilder);
        }
    }
}
