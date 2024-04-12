using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests.Excels
{
    public class GetRolesExcelRequest
    {
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string Name { get; set; } = string.Empty;
    }
}
