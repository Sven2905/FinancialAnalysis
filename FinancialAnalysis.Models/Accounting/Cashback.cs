namespace FinancialAnalysis.Models.Accounting
{
    public class Cashback
    {
        public int CashbackId { get; set; }
        public decimal Percentage { get; set; }
        public int TimeValue { get; set; }
        public PayType PayType { get; set; }
    }
}