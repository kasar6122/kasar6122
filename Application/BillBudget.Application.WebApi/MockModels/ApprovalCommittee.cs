using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class ApprovalCommittee
    {
        public string Id { get; set; }
        public decimal ApprovalLimit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string EmployeeId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
    }
}
