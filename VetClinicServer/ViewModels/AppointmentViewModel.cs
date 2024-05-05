using VetClinicServer.Models.Enums;

namespace VetClinicServer.ViewModels
{
    public class AppointmentViewModel
    {
        public int Id { get; set; } = 0;
        public required DisplayModel Procedure { get; set; }
        public required DisplayModel Pet { get; set; }
        public required DisplayModel Doctor { get; set; }
        public List<AppointmentDrugViewModel>? Drugs { get; set; }
        public DateTime DateAppointment { get; set; }
        public AppointmentStatuses? Status { get; set; }
        public DateTime? DateCompleted { get; set; }
        public decimal? TotalCost { get; set; }
        public string? Description { get; set; }
    }
}
