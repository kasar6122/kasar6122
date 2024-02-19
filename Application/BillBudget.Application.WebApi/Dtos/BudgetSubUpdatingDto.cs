using System;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class BudgetSubUpdatingDto
    {
        [Required]
        public string Id { get; set; }
        public DateTime UpdateddOn { get; set; }
        public string UpdatedBy { get; set; }

        [Required]
        public string BudgetMainId { get; set; }

        [Required]
        public string ItemName { get; set; }


        [Range(1, 10000000)]
        public decimal Quantity { get; set; }

        //[Range(ushort.MinValue, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public string MeasurementUnitId { get; set; }
        public int? VendorId { get; set; }
        public string WorkOrderNo { get; set; }
        public string QoutationNo { get; set; }

        [Range(1, double.MaxValue)]
        public decimal BudgetAmount { get; set; }

        [Range(1, double.MaxValue)]
        public decimal BudgetApproveAmount { get; set; }
        public bool? IsRegisterVendor { get; set; }
        public string VendorName { get; set; }

        public bool IsValidAmount() => BudgetAmount >= BudgetApproveAmount;
    }
}
