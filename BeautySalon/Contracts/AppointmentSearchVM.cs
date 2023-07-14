namespace BeautySalon.Contracts
{
    public class AppointmentSearchVM
    {
        public int CatalogId { get; set; }
        public int ServiceId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsUnapproved { get; set; }
        public bool IsFutureAppointment { get; set; }
        public bool IsPastAppointment { get; set; }
    }
}
