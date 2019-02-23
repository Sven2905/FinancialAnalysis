using FinancialAnalysis.Models.BaseClasses;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Haben
    /// </summary>
    public class Credit : CreditDebitBase
    {
        public Credit()
        {
        }

        public Credit(decimal Amount, int RefCostAccountId, int RefBookingId)
        {
            this.Amount = Amount;
            this.RefCostAccountId = RefCostAccountId;
            this.RefBookingId = RefBookingId;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int CreditId { get; set; }
    }
}