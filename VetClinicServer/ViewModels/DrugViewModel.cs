namespace VetClinicServer.ViewModels
{
    public class DrugViewModel
    {
        public int Id { get; set; } = 0;
        public required string Name { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }
}
