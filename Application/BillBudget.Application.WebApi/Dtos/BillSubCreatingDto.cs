using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class BillSubCreatingDto
    {
        public string Id { get; set; }
        public string BillMainId { get; set; }

        [Required, MaxLength(250)]
        public string ItemName { get; set; }

        public decimal Quantity { get; set; }
        public string MeasurementUnitId { get; set; }
        public decimal UnitPrice { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal BillAmount { get; set; }


        [Range(0.0, double.MaxValue)]
        public decimal BillApproveAmount { get; set; }
    }
}