namespace BillBudget.Application.WebApi.Helpers
{
    public class OtherOptions
    {
        public string OtherDeduction { get; set; }
        public bool CanCreateJournalAfterAnyReceiveOrPaymentVoucher { get; set; }
        public string SuperApprovalAuthorities { get; set; }
        public string BudgetApprovalUrl { get; set; }
        public string BillApprovalUrl { get; set; }
        public string AdvanceApprovalUrl { get; set; }
        public string BudgetApprovalProcessUrl { get; set; }
        public string BillApprovalProcessUrl { get; set; }
        public string AdvanceApprovalProcessUrl { get; set; }

    }
}
