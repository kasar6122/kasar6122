using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BudgetMain
    {
        public BudgetMain()
        {
            BudgetSub = new HashSet<BudgetSub>();
        }

        public string Id { get; set; }
        public bool Active { get; set; }
        public string ApplicationStatus { get; set; }
        public string AssignTo { get; set; }
        public string BudgetName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string JobId { get; set; }
        public decimal TotalApprovedAmount { get; set; }
        public decimal TotalBudgetAmount { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsApproved { get; set; }
        public string ForwardTo { get; set; }
        public string Note { get; set; }

        public Employee AssignToNavigation { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public Employee ForwardToNavigation { get; set; }
        public Job Job { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<BudgetSub> BudgetSub { get; set; }
    }
}
