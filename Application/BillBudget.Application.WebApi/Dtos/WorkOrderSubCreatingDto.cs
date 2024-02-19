using System.ComponentModel.DataAnnotations;

namespace BillBudget.Application.WebApi.Dtos
{
    public class WorkOrderSubCreatingDto
    {
        public string Id { get; set; }
        public string WorkOrderMainId { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required, Range(1, 1000000)]
        public decimal Quantity { get; set; }
        public string MeasurementUnitId { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
