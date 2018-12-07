namespace FinancialAnalysis.Models.Accounting
{
    public class Cashback
    {
        public int CashbackId { get; set; }
        public decimal Percent { get; set; }
        public int TimeValue { get; set; }
        public PayType PayType { get; set; }
    }
}