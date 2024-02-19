using DaffodilSoftware.Pagination.Sql;

namespace BillBudget.Application.WebApi.Dtos.QueryDtos
{
    public class CostCenterQueryParams: ResourceQueryParameters
    {
        public bool? Status { get; set; }
        public string Term { get; set; }
    }
}
