using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class Employee
    {
        public Employee()
        {
            AdvanceMainAssignToNavigation = new HashSet<AdvanceMain>();
            AdvanceMainCreatedByNavigation = new HashSet<AdvanceMain>();
            AdvanceMainForwardToNavigation = new HashSet<AdvanceMain>();
            AdvanceMainUpdatedByNavigation = new HashSet<AdvanceMain>();
            AdvanceSubCreatedByNavigation = new HashSet<AdvanceSub>();
            AdvanceSubUpdatedByNavigation = new HashSet<AdvanceSub>();
            ApprovalAssignToNavigation = new HashSet<Approval>();
            ApprovalCommitteeCreatedByNavigation = new HashSet<ApprovalCommittee>();
            ApprovalCommitteeUpdatedByNavigation = new HashSet<ApprovalCommittee>();
            ApprovalCreatedByNavigation = new HashSet<Approval>();
            ApprovalForwardToNavigation = new HashSet<Approval>();
            
            ApprovalPurposeCreatedByNavigation = new HashSet<ApprovalPurpose>();
            ApprovalPurposeUpdatedByNavigation = new HashSet<ApprovalPurpose>();
            ApprovalUpdatedByNavigation = new HashSet<Approval>();
            BillMainAssignToNavigation = new HashSet<BillMain>();
            BillMainCreatedByNavigation = new HashSet<BillMain>();
            BillMainForwardToNavigation = new HashSet<BillMain>();
            BillMainUpdatedByNavigation = new HashSet<BillMain>();
            BillSub = new HashSet<BillSub>();
            BudgetMainAssignToNavigation = new HashSet<BudgetMain>();
            BudgetMainCreatedByNavigation = new HashSet<BudgetMain>();
            BudgetMainForwardToNavigation = new HashSet<BudgetMain>();
            BudgetMainUpdatedByNavigation = new HashSet<BudgetMain>();
            BudgetPerformTypeCreatedByNavigation = new HashSet<BudgetPerformType>();
            BudgetPerformTypeUpdatedByNavigation = new HashSet<BudgetPerformType>();
            BudgetSubCreatedByNavigation = new HashSet<BudgetSub>();
            BudgetSubUpdatedByNavigation = new HashSet<BudgetSub>();
            CostCenterCreatedByNavigation = new HashSet<CostCenter>();
            CostCenterUpdatedByNavigation = new HashSet<CostCenter>();
            FinancialTransactionDetailsCreatedByNavigation = new HashSet<FinancialTransactionDetails>();
            FinancialTransactionDetailsUpdatedByNavigation = new HashSet<FinancialTransactionDetails>();
            FinancialTransactionTypeCreatedByNavigation = new HashSet<FinancialTransactionType>();
            FinancialTransactionTypeUpdatedByNavigation = new HashSet<FinancialTransactionType>();
            InverseReportsToNavigation = new HashSet<Employee>();
            JobAssignToNavigation = new HashSet<Job>();
            JobCreatedByNavigation = new HashSet<Job>();
            JobJobSupervisor = new HashSet<Job>();
            JobUpdatedByNavigation = new HashSet<Job>();
            MeasurementUnitCreatedByNavigation = new HashSet<MeasurementUnit>();
            MeasurementUnitUpdatedByNavigation = new HashSet<MeasurementUnit>();
            WorkOrderMainCreatedByNavigation = new HashSet<WorkOrderMain>();
            WorkOrderMainUpdatedByNavigation = new HashSet<WorkOrderMain>();
            WorkOrderSubCreatedByNavigation = new HashSet<WorkOrderSub>();
            WorkOrderSubUpdatedByNavigation = new HashSet<WorkOrderSub>();
            WorkforProxyEmployee = new HashSet<WorkforProxy>();
            WorkforProxyWorkfor = new HashSet<WorkforProxy>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CurrentDepartmentName { get; set; }
        public string Designation { get; set; }
        public int? PersonId { get; set; }
        public string Blocked { get; set; }
        public string Active { get; set; }
        public string ReportsTo { get; set; }

        public Employee ReportsToNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMainAssignToNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMainCreatedByNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMainForwardToNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMainUpdatedByNavigation { get; set; }
        public ICollection<AdvanceSub> AdvanceSubCreatedByNavigation { get; set; }
        public ICollection<AdvanceSub> AdvanceSubUpdatedByNavigation { get; set; }
        public ICollection<Approval> ApprovalAssignToNavigation { get; set; }
        public ICollection<ApprovalCommittee> ApprovalCommitteeCreatedByNavigation { get; set; }
        public ICollection<ApprovalCommittee> ApprovalCommitteeUpdatedByNavigation { get; set; }
        public ICollection<Approval> ApprovalCreatedByNavigation { get; set; }
        
        public ICollection<Approval> ApprovalForwardToNavigation { get; set; }
        public ICollection<ApprovalPurpose> ApprovalPurposeCreatedByNavigation { get; set; }
        public ICollection<ApprovalPurpose> ApprovalPurposeUpdatedByNavigation { get; set; }
        public ICollection<Approval> ApprovalUpdatedByNavigation { get; set; }
        public ICollection<BillMain> BillMainAssignToNavigation { get; set; }
        public ICollection<BillMain> BillMainCreatedByNavigation { get; set; }
        public ICollection<BillMain> BillMainForwardToNavigation { get; set; }
        public ICollection<BillMain> BillMainUpdatedByNavigation { get; set; }
        public ICollection<BillSub> BillSub { get; set; }
        public ICollection<BudgetMain> BudgetMainAssignToNavigation { get; set; }
        public ICollection<BudgetMain> BudgetMainCreatedByNavigation { get; set; }
        public ICollection<BudgetMain> BudgetMainForwardToNavigation { get; set; }
        public ICollection<BudgetMain> BudgetMainUpdatedByNavigation { get; set; }
        public ICollection<BudgetPerformType> BudgetPerformTypeCreatedByNavigation { get; set; }
        public ICollection<BudgetPerformType> BudgetPerformTypeUpdatedByNavigation { get; set; }
        public ICollection<BudgetSub> BudgetSubCreatedByNavigation { get; set; }
        public ICollection<BudgetSub> BudgetSubUpdatedByNavigation { get; set; }
        public ICollection<CostCenter> CostCenterCreatedByNavigation { get; set; }
        public ICollection<CostCenter> CostCenterUpdatedByNavigation { get; set; }
        public ICollection<FinancialTransactionDetails> FinancialTransactionDetailsCreatedByNavigation { get; set; }
        public ICollection<FinancialTransactionDetails> FinancialTransactionDetailsUpdatedByNavigation { get; set; }
        public ICollection<FinancialTransactionType> FinancialTransactionTypeCreatedByNavigation { get; set; }
        public ICollection<FinancialTransactionType> FinancialTransactionTypeUpdatedByNavigation { get; set; }
        public ICollection<Employee> InverseReportsToNavigation { get; set; }
        public ICollection<Job> JobAssignToNavigation { get; set; }
        public ICollection<Job> JobCreatedByNavigation { get; set; }
        public ICollection<Job> JobJobSupervisor { get; set; }
        public ICollection<Job> JobUpdatedByNavigation { get; set; }
        public ICollection<MeasurementUnit> MeasurementUnitCreatedByNavigation { get; set; }
        public ICollection<MeasurementUnit> MeasurementUnitUpdatedByNavigation { get; set; }
        public ICollection<WorkOrderMain> WorkOrderMainCreatedByNavigation { get; set; }
        public ICollection<WorkOrderMain> WorkOrderMainUpdatedByNavigation { get; set; }
        public ICollection<WorkOrderSub> WorkOrderSubCreatedByNavigation { get; set; }
        public ICollection<WorkOrderSub> WorkOrderSubUpdatedByNavigation { get; set; }
        public ICollection<WorkforProxy> WorkforProxyEmployee { get; set; }
        public ICollection<WorkforProxy> WorkforProxyWorkfor { get; set; }
    }
}
