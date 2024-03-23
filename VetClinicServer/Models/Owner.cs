using System.ComponentModel.DataAnnotations;

namespace VetClinicServer.Models
{
    public class Owner
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string FIO { get; set; }
        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
        [MaxLength(255)]
        public string? Email { get; set; }

        public virtual ICollection<Pet>? Pets { get; set; }
    }
}
