namespace BillBudget.Application.WebApi.Dtos.QueryDtos
{
    public class TransactionQueryDto
    {
        public string RefrenceId { get; set; }
        public string VoucherNo { get; set; }
        public string JobId { get; set; }
        public string Id { get; set; }
        public string TransactionTypeId { get; set; }
        public string WorkOrderId { get; set; }
        public bool IsForAdjustment { get; set; }
    }
}
