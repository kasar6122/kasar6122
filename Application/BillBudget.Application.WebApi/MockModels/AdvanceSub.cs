using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class AdvanceSub
    {
        public string Id { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal AdvanceApproveAmount { get; set; }
        public string AdvanceMainId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ItemName { get; set; }
        public string MeasurementUnitId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public AdvanceMain AdvanceMain { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public MeasurementUnit MeasurementUnit { get; set; }
        public Employee UpdatedByNavigation { get; set; }
    }
}
