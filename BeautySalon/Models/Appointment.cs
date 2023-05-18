using System.ComponentModel.DataAnnotations.Schema;

namespace BeautySalon.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime FinishDateTime { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public bool Approved { get; set; }

        public bool Canceled { get; set; }

    }
}
