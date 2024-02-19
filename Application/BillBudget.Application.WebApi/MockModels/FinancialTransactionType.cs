using System;
using System.Collections.Generic;

namespace BillBudget.Application.WebApi.MockModels
{
    public partial class FinancialTransactionType
    {
        public FinancialTransactionType()
        {
            FinancialTransactionDetails = new HashSet<FinancialTransactionDetails>();
        }

        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public Employee CreatedByNavigation { get; set; }
        public Employee UpdatedByNavigation { get; set; }
        public ICollection<FinancialTransactionDetails> FinancialTransactionDetails { get; set; }
    }
}
