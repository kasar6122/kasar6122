using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillBudget.Application.WebApi.Dtos
{
    public class AdvanceMainCreatingDto
    {
        [Required]
        public string JobId { get; set; }

        public string WorkOrderMainId { get; set; }

        public DateTime? AdvanceDate { get; set; }
        public int? AdvanceNumber { get; set; }

        [Required, MinLength(3), MaxLength(250)]
        public string AdvanceName { get; set; }
        public bool IsApproved { get; set; }
        public string Note { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorDetail { get; set; }
        public string QuotationNo { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        public decimal TotalApproveAmount { get; set; }

        [Required]
        public string AssignTo { get; set; }

        [Required]
        public string ForwardTo { get; set; }

        public string ApplicationStatus { get; set; }
        public string UpdatedFromIP { get; set; }


       // [MustHaveOneElement(ErrorMessage = "At least one item is required")]
        public virtual ICollection<AdvanceSubCreatingDto> AdvanceSub { get; set; }

        public bool HasDuplicate() => this.AdvanceSub.GroupBy(g => g.ItemName).Any(a => a.Count() > 1);

        public void SetDefaultValue()
        {
            this.AdvanceSub.ToList().ForEach(a =>
            {
                var total = (a.Quantity ?? 0) * (a.UnitPrice ?? 0);
                a.AdvanceAmount = total;
                a.AdvanceApproveAmount = total;
            });
            this.TotalAmount = this.AdvanceSub.Sum(a => a.AdvanceAmount);
            this.TotalApproveAmount = this.AdvanceSub.Sum(a => a.AdvanceApproveAmount);
        }
    }
}
