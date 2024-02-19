using System;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class JobCreatingDto
    {
        [Required]
        public string CostCenterId { get; set; }

        [Required, MinLength(3), MaxLength(250)]
        public string JobName { get; set; }

        public string JobDescription { get; set; }
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime StartedDate { get; set; }
        public string Status { get; set; }

        [Required]
        public DateTime EstimetedEndDate { get; set; }

        public DateTime? ActualEndDate { get; set; }
        public string ReferenceNo { get; set; }

        [Required]
        public string AssignTo { get; set; }

        public decimal? TotalBudget { get; set; }
        public decimal? TotalAdvance { get; set; }
        public decimal? TotalBill { get; set; }
        public decimal? TotalAdjust { get; set; }
        public string Note { get; set; }
        public string JobSupervisorId { get; set; }


    }
}
