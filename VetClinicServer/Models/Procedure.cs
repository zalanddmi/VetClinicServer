using System.ComponentModel.DataAnnotations;

namespace VetClinicServer.Models
{
    public class Procedure
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
    }
}
