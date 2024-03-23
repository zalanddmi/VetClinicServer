using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinicServer.Models
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public required string UserName { get; set; }
        [MaxLength(255)]
        public required string FIO { get; set; }
        [MaxLength(20)]
        public required string PhoneNumber { get; set; }
        [MaxLength(255)]
        public required string Email { get; set; }
        [MaxLength(255)]
        public required string PasswordHash { get; set; }
        [MaxLength(255)]
        public required string Salt { get; set; }
        public int PostId { get; set; }
        [ForeignKey("PostId")]
        public required Post Post { get; set; }
    }
}
