using DaffodilSoftware.Pagination.Sql;

namespace BillBudget.Application.WebApi.Dtos.QueryDtos
{
    public class JobQueryParams : ResourceQueryParameters
    {
        public string Term { get; set; }
        public string CostCenterId { get; set; }
        public string AssignTo { get; set; }
        public string Status { get; set; }
    }
}
