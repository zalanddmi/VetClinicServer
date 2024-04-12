using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests.Pages
{
    public class GetPagedSpeciesRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string Name { get; set; } = string.Empty;
    }
}
