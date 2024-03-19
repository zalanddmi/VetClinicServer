using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests
{
    public class GetPagedDrugsRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; } = 0;
        public ComparisonOperators CostComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
        public int Quantity { get; set; } = 0;
        public ComparisonOperators QuantityComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
    }
}
