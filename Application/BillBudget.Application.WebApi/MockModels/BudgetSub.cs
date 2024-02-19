using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BudgetSub
    {
        public string Id { get; set; }
        public decimal? BudgetAmount { get; set; }
        public decimal? BudgetApproveAmount { get; set; }
        public string BudgetMainId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsRegisterVendor { get; set; }
        public string ItemName { get; set; }
        public string MeasurementUnitId { get; set; }
        public string QoutationNo { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string WorkOrderNo { get; set; }

        public BudgetMain BudgetMain { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public Employee UpdatedByNavigation { get; set; }
    }
}
