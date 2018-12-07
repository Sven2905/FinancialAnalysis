namespace FinancialAnalysis.Models.Accounting
{
    public class PaymentCondition
    {
        public int PaymentConditionId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercent { get; set; }
        public int RefCashbackId { get; set; }
        public Cashback Cashback { get; set; }
    }
}