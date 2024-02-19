using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class TransactionDetailsDto
    {
        [Required]
        public string JobId { get; set; }

        [Required]
        public string ReferenceId { get; set; }

        [Required]
        public string FinancialTransactionTypeId { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public string VoucherNo { get; set; }
        public bool IsForAdjustment { get; set; }

        public string WorkOrderMainId { get; set; }

        [Required]
        public string ApprovalPurposeId { get; set; }
    }
}
