using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class FinancialTransactionDetails
    {
        public string Id { get; set; }
        public decimal Cr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal Dr { get; set; }
        public string FinancialTransactionTypeId { get; set; }
        public string JobId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string VoucherNo { get; set; }
        public string ReferenceId { get; set; }
        public bool IsForAdjustment { get; set; }
        public string AccountHead { get; set; }
        public string WorkOrderMainId { get; set; }
        public bool IsDeductionEntry { get; set; }
        public string BillType { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public FinancialTransactionType FinancialTransactionType { get; set; }
        public Job Job { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public WorkOrderMain WorkOrderMain { get; set; }
    }
}
