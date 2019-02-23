using FinancialAnalysis.Models.BaseClasses;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Soll
    /// </summary>
    public class Debit : CreditDebitBase
    {
        public Debit()
        {
        }

        public Debit(decimal Amount, int RefCostAccountId, int RefBookingId)
        {
            this.Amount = Amount;
            this.RefCostAccountId = RefCostAccountId;
            this.RefBookingId = RefBookingId;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int DebitId { get; set; }
    }
}