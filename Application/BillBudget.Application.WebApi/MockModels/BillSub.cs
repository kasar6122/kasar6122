using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BillSub
    {
        public string Id { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? BillApproveAmount { get; set; }
        public string BillMainId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ItemName { get; set; }
        public string MeasurementUnitId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public BillMain BillMain { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public Employee UpdatedByNavigation { get; set; }
    }
}
