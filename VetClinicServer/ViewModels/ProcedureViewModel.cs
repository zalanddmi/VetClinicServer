namespace VetClinicServer.ViewModels
{
    public class ProcedureViewModel
    {
        public int Id { get; set; } = 0;
        public required string Name { get; set; }
        public decimal Cost { get; set; }
        public string? Description { get; set; }
    }
}
