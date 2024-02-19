using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class Job
    {
        public Job()
        {
            AdvanceMain = new HashSet<AdvanceMain>();
            Approval = new HashSet<Approval>();
            BillMain = new HashSet<BillMain>();
            BudgetMain = new HashSet<BudgetMain>();
            FinancialTransactionDetails = new HashSet<FinancialTransactionDetails>();
            WorkOrderMain = new HashSet<WorkOrderMain>();
        }

        public string Id { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string AssignTo { get; set; }
        public string CostCenterId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? EstimetedEndDate { get; set; }
        public bool IsActive { get; set; }
        public string JobDescription { get; set; }
        public string JobName { get; set; }
        public string JobSupervisorId { get; set; }
        public string Note { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime? StartedDate { get; set; }
        public decimal? TotalAdjust { get; set; }
        public decimal? TotalAdvance { get; set; }
        public decimal? TotalBill { get; set; }
        public decimal? TotalBudget { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Status { get; set; }

        public Employee AssignToNavigation { get; set; }
        public CostCenter CostCenter { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public Employee JobSupervisor { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMain { get; set; }
        public ICollection<Approval> Approval { get; set; }
        public ICollection<BillMain> BillMain { get; set; }
        public ICollection<BudgetMain> BudgetMain { get; set; }
        public ICollection<FinancialTransactionDetails> FinancialTransactionDetails { get; set; }
        public ICollection<WorkOrderMain> WorkOrderMain { get; set; }
    }
}
