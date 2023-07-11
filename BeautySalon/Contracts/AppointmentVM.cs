using BeautySalon.Models;
using System.ComponentModel.DataAnnotations;

namespace BeautySalon.Contracts
{
    public class AppointmentVM
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime FinishDateTime { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public bool Approved { get; set; }
        public bool Canceled { get; set; }

    }
}
