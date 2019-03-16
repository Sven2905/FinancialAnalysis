using FinancialAnalysis.Models.BaseClasses;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Soll
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
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