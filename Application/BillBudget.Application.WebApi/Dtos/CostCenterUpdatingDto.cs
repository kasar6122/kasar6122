using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class CostCenterUpdatingDto : CostCenterCreatingDto
    {
        [Required]
        public string Id { get; set; }
    }
}