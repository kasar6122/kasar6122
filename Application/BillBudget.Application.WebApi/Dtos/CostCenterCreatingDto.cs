using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class CostCenterCreatingDto
    {
        [Required, MinLength(3), MaxLength(250)]
        public string CostCenterName { get; set; }
        public string Location { get; set; }
        public string ApprovalAuthority { get; set; }
    }
}
