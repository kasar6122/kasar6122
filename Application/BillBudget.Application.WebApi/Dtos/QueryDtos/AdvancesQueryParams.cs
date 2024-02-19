using DaffodilSoftware.Pagination.Sql;

namespace BillBudget.Application.WebApi.Dtos.QueryDtos
{
    public class AdvancesQueryParams : ResourceQueryParameters
    {
        public string Id { get; set; }
        public string JobId { get; set; }
    }
}
