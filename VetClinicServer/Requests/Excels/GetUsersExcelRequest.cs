using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests.Excels
{
    public class GetUsersExcelRequest
    {
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string UserName { get; set; } = string.Empty;
        public string FIO { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Post { get; set; } = string.Empty;
    }
}
