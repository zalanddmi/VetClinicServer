using System.ComponentModel.DataAnnotations;

namespace VetClinicServer.Models
{
    public class Drug
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }

        public ICollection<AppointmentDrug>? AppointmentDrugs { get; set; }
    }
}
