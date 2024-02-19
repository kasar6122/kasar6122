using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BillBudget.Application.WebApi.Areas.Accounts.Models;

namespace BillBudget.Application.WebApi.Dtos
{
    public class VoucherWithFinancialTransactionDto
    {
        public VoucherWithFinancialTransactionDto()
        {
            this.VoucherDetails = new List<VoucherSubCreatingDto>();
        }

        [MaxLength(1000)]
        public string Narration { get; set; }

        public DateTime? Date { get; set; } = DateTime.Now;

        [Required, MaxLength(10)]
        public string PrjCode { get; set; }

        [MinLength(1), MaxLength(1)]
        public string V_Lock { get; set; } = "F";

        [MinLength(1), MaxLength(1)]
        public string Active { get; set; } = "T";

        [MinLength(1), MaxLength(1)]
        public string Cancelled { get; set; } = "F";
        public DateTime? Ent_Date { get; set; } = DateTime.Now;

        [MaxLength(1)]
        public string Curr_Convert { get; set; }

        [MaxLength(36), Required]
        public string Job_No { get; set; }

        [Required]
        public string VoucherType { get; set; }

        [Required]
        public string ReferenceId { get; set; }

        [Required]
        public string ApprovalPurposeId { get; set; }
        public string JobId { get; set; }

        public decimal OtherDeduction { get; set; }

        [Required]
        public string FinancialTransactionTypeId { get; set; }
        public string WorkOrderMainId { get; set; }
        public decimal Dr { get; set; }
        public decimal Cr { get; set; }
        public string VoucherNo { get; set; }

        // [MustHaveOneElement(ErrorMessage = "Sorry, no voucher entry found so save.")]
        public List<VoucherSubCreatingDto> VoucherDetails { get; set; }

        public bool IsEqualDebitAndCreditAmount() => this.VoucherDetails.Sum(s => s.Dr_amount - s.Cr_amount) == 0;

        public (decimal DebitAmount, decimal CreditAmount) CalculateDebitAndCreditAmount() =>
            (this.VoucherDetails.Sum(s => s.Dr_amount), this.VoucherDetails.Sum(s => s.Dr_amount));

        public void SetDefaultValue()
        {
            this.JobId = this.Job_No;

            this.VoucherDetails.ForEach(v =>
             {
                 v.Job_No = this.Job_No;
             });
            this.Dr = this.VoucherDetails.Sum(s => s.Dr_amount);
            this.Cr = this.VoucherDetails.Sum(s => s.Cr_amount);
        }

        public bool HasDuplicateAccountCode() => this.VoucherDetails.GroupBy(x => x.Ac_Code).Any(g => g.Count() > 1);
    }
}
