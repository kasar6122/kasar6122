using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class BillMain
    {
        public BillMain()
        {
            BillSub = new HashSet<BillSub>();
        }

        public string Id { get; set; }
        public decimal? AdjustmentAmount { get; set; }
        public string ApplicationStatus { get; set; }
        public string AssignTo { get; set; }
        public DateTime? BillDate { get; set; }
        public string BillName { get; set; }
        public decimal? BillTotalAmount { get; set; }
        public decimal? BillTotalApproveAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal? DeductionByAccounts { get; set; }
        public decimal? Discount { get; set; }
        public bool? IsRegisterVendor { get; set; }
        public string JobId { get; set; }
        public string Note { get; set; }
        public string QoutationNo { get; set; }
        public decimal? TaxAmount { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string VoucherNo { get; set; }
        public bool IsApproved { get; set; }
        public string ForwardTo { get; set; }
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalReceivedAmount { get; set; }
        public string WorkOrderMainId { get; set; }
        public decimal? ReturnedAmount { get; set; }

        public Employee AssignToNavigation { get; set; }
        public Employee CreatedByNavigation { get; set; }
        public Employee ForwardToNavigation { get; set; }
        public Job Job { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public WorkOrderMain WorkOrderMain { get; set; }
        public ICollection<BillSub> BillSub { get; set; }
    }
}
