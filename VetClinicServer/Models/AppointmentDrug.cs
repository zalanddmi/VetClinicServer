using System.ComponentModel.DataAnnotations;

namespace VetClinicServer.Models
{
    public class AppointmentDrug
    {
        [Key]
        public int AppointmentId { get; set; }
        [Key]
        public int DrugId { get; set; }
        public int Quantity { get; set; }
        public required Appointment Appointment { get; set; }
        public required Drug Drug { get; set; }
    }
}
