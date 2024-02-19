using System;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
   public class JobUpdatingDto: JobCreatingDto
    {
        [Required]
        public string Id { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
