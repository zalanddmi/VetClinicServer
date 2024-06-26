﻿using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests.Excels
{
    public class GetDrugsExcelRequest
    {
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; } = 0;
        public ComparisonOperators CostComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
        public int Quantity { get; set; } = 0;
        public ComparisonOperators QuantityComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
    }
}
