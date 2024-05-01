namespace VetClinicServer.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string FIO { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required DisplayModel Post { get; set; }
        public string? Password { get; set; }
    }
}
