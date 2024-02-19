using DaffodilSoftware.Pagination.Sql;

namespace BillBudget.Application.WebApi.Dtos.QueryDtos
{
    public class BudgetQueryParams : ResourceQueryParameters
    {
        public bool? Status { get; set; }
        public string CostCenterId { get; set; }
        public string JobId { get; set; }
        public string AssignTo { get; set; }
    }
}
