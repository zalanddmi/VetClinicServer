using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinicServer.Models
{
    public class Post
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual required Role Role { get; set; }
    }
}
