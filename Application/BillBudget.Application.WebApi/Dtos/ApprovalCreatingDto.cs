using BillBudget.Application.WebApi.MockModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class ApprovalCreatingDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string JobId { get; set; }

        [Required]
        public string ApprovalPurposeId { get; set; }

        [Required]
        public string Ref_Id { get; set; }
        public string Ref_Name { get; set; }
        public string ForwardTo { get; set; }
        
        public string Note { get; set; }

        [Required]
        public string BudgetPerformTypeId { get; set; }

        public bool? IsApproved { get; set; } = false;
        public string VarificationKey { get; set; }

        [Required]
        public string AssignTo { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
