using VetClinicServer.Models.Enums;
using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests.Excels
{
    public class GetAppointmentsExcelRequest
    {
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string Procedure { get; set; } = string.Empty;
        public string Pet { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public DateTime DateAppointment { get; set; } = DateTime.MinValue;
        public ComparisonOperators DateAppointmentComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
        public AppointmentStatuses? Status { get; set; }
        public DateTime? DateCompleted { get; set; }
        public ComparisonOperators DateCompletedComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
        public decimal TotalCost { get; set; } = 0;
        public ComparisonOperators TotalCostComparisonOperators { get; set; } = ComparisonOperators.GreaterThanOrEqual;
    }
}
