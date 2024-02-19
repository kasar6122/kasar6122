using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class CostCenter
    {
        public CostCenter()
        {
            Job = new HashSet<Job>();
        }

        public string Id { get; set; }
        public string CostCenterName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public string Location { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ApprovalAuthority { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<Job> Job { get; set; }
    }
}
