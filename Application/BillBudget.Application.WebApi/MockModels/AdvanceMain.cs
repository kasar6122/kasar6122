using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class AdvanceMain
    {
        public AdvanceMain()
        {
            AdvanceSub = new HashSet<AdvanceSub>();
        }

        public string Id { get; set; }
        public DateTime AdvanceDate { get; set; }
        public string AdvanceName { get; set; }
        public int? AdvanceNumber { get; set; }
        public string ApplicationStatus { get; set; }
        public string AssignTo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string JobId { get; set; }
        public string Note { get; set; }
        public string QuotationNo { get; set; }
        public decimal? TotalAmount { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string VendorDetail { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public bool IsApproved { get; set; }
        public string ForwardTo { get; set; }
        public string WorkOrderMainId { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalApproveAmount { get; set; }

        public Employee AssignToNavigation { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public Employee ForwardToNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public Job Job { get; set; }
        
        public WorkOrderMain WorkOrderMain { get; set; }
        public ICollection<AdvanceSub> AdvanceSub { get; set; }
    }
}
