using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BudgetPerformType
    {
        public BudgetPerformType()
        {
            Approval = new HashSet<Approval>();
        }

        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TypeName { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<Approval> Approval { get; set; }
    }
}
