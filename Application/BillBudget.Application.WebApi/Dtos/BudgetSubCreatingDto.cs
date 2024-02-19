using System;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class BudgetSubCreatingDto
    {
        public string Id { get; set; }

        public string BudgetMainId { get; set; }

        [Required, MinLength(1), MaxLength(250)]
        public string ItemName { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Quantity { get; set; }

        //[Range(short.MinValue, double.MaxValue)]
        public decimal UnitPrice { get; set; }
        public int? VendorId { get; set; }
        public string WorkOrderNo { get; set; }
        public string QoutationNo { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal BudgetAmount { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal BudgetApproveAmount { get; set; }
        public string UpdatedFromIP { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsRegisterVendor { get; set; }
        public string VendorName { get; set; }
        public string Unit { get; set; }

        [Range(1, int.MaxValue)]
        public string MeasurementUnitId { get; set; }
    }
}
