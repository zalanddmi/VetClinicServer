using VetClinicServer.Utils;

namespace VetClinicServer.ViewModels
{
    public class PaginatedListDTO<T>
    {
        public required PaginatedList<T> Items { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
    }
}
