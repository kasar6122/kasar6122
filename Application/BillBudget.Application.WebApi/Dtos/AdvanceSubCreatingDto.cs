using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class AdvanceSubCreatingDto
    {
        public string Id { get; set; }
        [Required]
        public string ItemName { get; set; }

        public decimal? Quantity { get; set; }
        public string MeasurementUnitId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string UpdatedFromIP { get; set; }
        public string UpdatedBy { get; set; }

        public string AdvanceMainId { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal AdvanceAmount { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal AdvanceApproveAmount { get; set; }
    }
}
