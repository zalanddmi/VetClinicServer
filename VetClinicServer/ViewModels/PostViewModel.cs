namespace VetClinicServer.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DisplayModel Role { get; set; }
        public string? Description { get; set; }
    }
}
