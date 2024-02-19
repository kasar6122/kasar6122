using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class MeasurementUnit
    {
        public MeasurementUnit()
        {
            AdvanceSub = new HashSet<AdvanceSub>();
            BillSub = new HashSet<BillSub>();
            BudgetSub = new HashSet<BudgetSub>();
            WorkOrderSub = new HashSet<WorkOrderSub>();
        }

        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Unit { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<AdvanceSub> AdvanceSub { get; set; }
        public ICollection<BillSub> BillSub { get; set; }
        public ICollection<BudgetSub> BudgetSub { get; set; }
        public ICollection<WorkOrderSub> WorkOrderSub { get; set; }
    }
}
