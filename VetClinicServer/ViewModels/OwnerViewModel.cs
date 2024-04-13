namespace VetClinicServer.ViewModels
{
    public class OwnerViewModel
    {
        public int Id { get; set; } = 0;
        public required string FIO { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
