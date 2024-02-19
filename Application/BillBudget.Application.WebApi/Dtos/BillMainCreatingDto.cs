using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillBudget.Application.WebApi.Dtos
{
    public class BillMainCreatingDto
    {
        public BillMainCreatingDto()
        {
            this.BillSub = new List<BillSubCreatingDto>();
        }

        [Required]
        public string JobId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime? BillDate { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(250)]
        public string BillName { get; set; }
        public string Note { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public decimal ReturnedAmount { get; set; }
        public string WorkOrderMainId { get; set; }
        public string QoutationNo { get; set; }
        public int? VendorId { get; set; }
        public bool IsRegisterVendor { get; set; }
        public string VendorName { get; set; }
        public decimal BillTotalAmount { get; set; }
        public decimal BillTotalApproveAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal BillPaidAmount { get; set; }

        [Required]
        public string AssignTo { get; set; }
        public string VoucherNo { get; set; }
        public string ApplicationStatus { get; set; }
        public decimal DeductionByAccounts { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        [Required]
        public string ForwardTo { get; set; }

        // [MustHaveOneElement(ErrorMessage = "At least one item is required")]
        public List<BillSubCreatingDto> BillSub { get; set; }

        public bool HasDuplicate() => BillSub.GroupBy(g => g.ItemName).Any(b => b.Count() > 1);

        public void SetDefaultValue()
        {
            this.BillSub
                 .ToList()
                 .ForEach(f =>
                 {
                     f.Id = string.IsNullOrWhiteSpace(f.Id) ? null : f.Id;
                     f.BillAmount = f.UnitPrice * f.Quantity;
                 });

            this.BillSub
                .ToList()
                .ForEach(f =>
                {
                    f.BillApproveAmount = f.BillAmount;
                });
            if (string.IsNullOrWhiteSpace(this.WorkOrderMainId))
                this.WorkOrderMainId = null;

            this.BillTotalAmount = this.BillSub.Sum(b => b.BillAmount);
            this.BillTotalApproveAmount = this.BillTotalAmount;
        }
    }
}