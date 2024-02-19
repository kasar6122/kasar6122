using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class WorkforProxy
    {
        public string EmployeeId { get; set; }
        public string WorkforId { get; set; }

        public Employee Employee { get; set; }
        public Employee Workfor { get; set; }
    }
}
