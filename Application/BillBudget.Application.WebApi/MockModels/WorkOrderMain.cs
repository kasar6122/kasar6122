using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class WorkOrderMain
    {
        public WorkOrderMain()
        {
            AdvanceMain = new HashSet<AdvanceMain>();
            BillMain = new HashSet<BillMain>();
            FinancialTransactionDetails = new HashSet<FinancialTransactionDetails>();
            WorkOrderSub = new HashSet<WorkOrderSub>();
        }

        public string Id { get; set; }
        public string ContactPerson { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DelivaryDateTime { get; set; }
        public string DelivaryPlace { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string JobId { get; set; }
        public string Note { get; set; }
        public string TermsCondition { get; set; }
        public decimal? TotalAmount { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string VendorDetail { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime? WorkOrderDate { get; set; }
        public string WorkOrderName { get; set; }
        public string WorkOrderNumber { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Job Job { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<AdvanceMain> AdvanceMain { get; set; }
        public ICollection<BillMain> BillMain { get; set; }
        public ICollection<FinancialTransactionDetails> FinancialTransactionDetails { get; set; }
        public ICollection<WorkOrderSub> WorkOrderSub { get; set; }
    }
}
