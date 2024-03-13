using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinicServer.Models
{
    public class Pet
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public int SpeciesId { get; set; }
        [ForeignKey("SpeciesId")]
        public required Species Species { get; set; }
        [MaxLength(255)]
        public required string Breed { get; set; }
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public required Owner Owner { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Description { get; set; }
    }
}
