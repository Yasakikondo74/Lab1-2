using System.ComponentModel.DataAnnotations;

namespace Lab1_2.Models
{
    public class Ticket
    {
        [Key]
        public Guid TicketID { get; set; }
        public string TicketName { get; set; }
        public Guid TheEventId { get; set; }
        public TheEvent TheEvent { get; set; }
    }
}
