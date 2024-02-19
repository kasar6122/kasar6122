using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Areas.Accounts.Models
{
    public class VoucherSubCreatingDto
    {

        [MaxLength(10)]
        public string DeptCode { get; set; }

        [Required, MaxLength(20)]
        public string Ac_Code { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal Dr_amount { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal Cr_amount { get; set; }

        [MaxLength(15)]
        public string Chq_no { get; set; }

        [MaxLength(15)]
        public string Bill_no { get; set; }

        [MaxLength(50)]
        public string Inv_no { get; set; }

        [MaxLength(15)]
        public string MR_no { get; set; }

        [MaxLength(36)]
        public string Job_No { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [Required, Range(1, 1000)]
        public int AccCode { get; set; }

        [Required]
        public string AccountHead { get; set; }

        public bool IsForTransaction { get; set; }

        public bool IsDeductionEntry { get; set; }
    }
}
