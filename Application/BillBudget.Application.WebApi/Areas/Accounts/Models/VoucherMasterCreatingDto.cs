using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BillBudget.Application.WebApi.Areas.Accounts.Models
{
    public class VoucherMasterCreatingDto
    {
        public VoucherMasterCreatingDto()
        {
            this.VoucherDetails = new List<VoucherSubCreatingDto>();
        }
        public string Voucher_no { get; set; }


        [MaxLength(1000)]
        public string Narration { get; set; }

        public DateTime? Date { get; set; } = DateTime.UtcNow;

        [Required, MaxLength(10)]
        public string PrjCode { get; set; }

        [MinLength(1), MaxLength(1)]
        public string V_Lock { get; set; } = "F";

        [MinLength(1), MaxLength(1)]
        public string Active { get; set; } = "T";

        [MinLength(1), MaxLength(1)]
        public string Cancelled { get; set; } = "F";
        public DateTime? Ent_Date { get; set; } = DateTime.UtcNow;

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

        [Required]
        public List<VoucherSubCreatingDto> VoucherDetails { get; set; }

        public bool IsEqualDebitAndCreditAmount() => this.VoucherDetails.Sum(s => s.Dr_amount - s.Cr_amount) == 0;

        public (decimal DebitAmount, decimal CreditAmount) CalculateDebitAndCreditAmount() =>
            (this.VoucherDetails.Sum(s => s.Dr_amount), this.VoucherDetails.Sum(s => s.Dr_amount));

        public void SetDefaultValue()
        {
            this.VoucherDetails.ForEach(v =>
            {
                v.Job_No = this.Job_No;
            });
        }


        public bool HasDuplicateAccountCode() =>
             this.VoucherDetails
                .GroupBy(x => x.Ac_Code)
                .Any(g => g.Count() > 1);

    }
}
