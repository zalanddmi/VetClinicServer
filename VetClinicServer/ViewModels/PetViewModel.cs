namespace VetClinicServer.ViewModels
{
    public class PetViewModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required DisplayModel Species { get; set; }
        public required string Breed { get; set; }
        public required DisplayModel Owner { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Description { get; set; }
    }
}
