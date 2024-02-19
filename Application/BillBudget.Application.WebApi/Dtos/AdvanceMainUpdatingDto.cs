using System;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class AdvanceMainUpdatingDto : AdvanceMainCreatingDto
    {
        [Required]
        public string Id { get; set; }
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; }
    }
}
