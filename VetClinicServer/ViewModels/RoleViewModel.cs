namespace VetClinicServer.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; } = 0;
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
