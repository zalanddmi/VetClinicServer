using VetClinicServer.Requests.Enums;

namespace VetClinicServer.Requests
{
    public class GetPagedUsersRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchString { get; set; } = string.Empty;
        public string OrderBy { get; set; } = "Id";
        public SortDirections SortDirection { get; set; } = SortDirections.Ascending;
        public string UserName { get; set; } = string.Empty;
        public string FIO { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PostName { get; set; } = string.Empty;
    }
}
