using System.ComponentModel.DataAnnotations;

namespace VetClinicServer.Models
{
    public class Species
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}
