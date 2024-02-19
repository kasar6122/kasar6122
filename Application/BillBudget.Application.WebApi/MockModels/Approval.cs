using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class Approval
    {
        public string Id { get; set; }
        public string ApprovalPurposeId { get; set; }
        public string AssignTo { get; set; }
        public string BudgetPerformTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ForwardTo { get; set; }
        public string JobId { get; set; }
        public string Note { get; set; }
        public string RefId { get; set; }
        
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string VarificationKey { get; set; }

        public ApprovalPurpose ApprovalPurpose { get; set; }
        public Employee AssignToNavigation { get; set; }
        public BudgetPerformType BudgetPerformType { get; set; }
        public Employee CreatedByNavigation { get; set; }

        public Employee ForwardToNavigation { get; set; }
        public Job Job { get; set; }
        public Employee UpdatedByNavigation { get; set; }
    }
}
