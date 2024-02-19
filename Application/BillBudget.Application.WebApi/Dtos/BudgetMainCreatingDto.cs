using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillBudget.Application.WebApi.Dtos
{
    public class BudgetMainCreatingDto
    {
        [Required, MinLength(3), MaxLength(250)]
        public string BudgetName { get; set; }
        public string CreatedIP { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; } = true;

        public bool IsApproved { get; set; }

        [Required]
        public string AssignTo { get; set; }

        [Required]
        public string ForwardTo { get; set; }
        public string ApplicationStatus { get; set; }

       // [MustHaveOneElement(ErrorMessage = "At least one item is required")]
        public IEnumerable<BudgetSubCreatingDto> BudgetSub { get; set; }
        public decimal? TotalBudgetAmount { get; set; }
        public decimal? TotalApprovedAmount { get; set; }

        [Required]
        public string JobId { get; set; }
        public string JobName { get; set; }
        public string CostCenterId { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterLocation { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }
        public bool HasDuplicate() => BudgetSub.GroupBy(g => g.ItemName).Any(b => b.Count() > 1);

        public void SetDefaultValue()
        {
            this.BudgetSub
                .ToList()
                .ForEach(f =>
                {
                    f.Id = string.IsNullOrWhiteSpace(f.Id) ? null : f.Id;
                    f.BudgetAmount = f.UnitPrice * f.Quantity;
                });

            this.BudgetSub
                .ToList()
                .ForEach(f =>
            {
                f.BudgetApproveAmount = f.BudgetAmount;
            });
            this.CreatedOn = DateTime.UtcNow;
            this.TotalBudgetAmount = BudgetSub.Sum(b => b.BudgetAmount);
            this.TotalApprovedAmount = this.TotalBudgetAmount;
        }
    }
}
