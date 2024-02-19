using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillBudget.Application.WebApi.Dtos
{
    public class WorkOrderMainCreatingDto
    {
        public WorkOrderMainCreatingDto()
        {
            this.WorkOrderSub = new List<WorkOrderSubCreatingDto>();
        }

        [Required]
        public string JobId { get; set; }

        [Required]
        public string WorkOrderNumber { get; set; }

        [Required, MinLength(3), MaxLength(250)]
        public string WorkOrderName { get; set; }
        public string Note { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorDetail { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string TermsCondition { get; set; }

        [Required]
        public DateTime DelivaryDateTime { get; set; }
        public string DelivaryPlace { get; set; }

        public string ContactPerson { get; set; }

        public DateTime? WorkOrderDate { get; set; } = DateTime.UtcNow;

      //  [MustHaveOneElement(ErrorMessage = "At least one item is required")]
        public List<WorkOrderSubCreatingDto> WorkOrderSub { get; set; }

        public bool HasDuplicate() => this.WorkOrderSub.GroupBy(g => g.ItemName).Any(a => a.Count() > 1);

        public void SetDefaultValue()
        {
            this.TotalAmount = this.WorkOrderSub.Sum(s => s.Quantity * s.UnitPrice);
        }
    }
}
