using System.ComponentModel.DataAnnotations;

namespace Lab1_2.Models
{
    public class TheEvent
    {
        [Key]
        public Guid EventID { get; set; }
        public string EventName { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public int TotalTickets { get; set; }
    }
}
