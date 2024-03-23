namespace VetClinicServer.Requests
{
    public class RegisterRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string FIO { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public int PostId { get; set; }
    }
}
