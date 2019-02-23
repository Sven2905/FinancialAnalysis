namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Zahlungskondition
    /// </summary>
    public class PaymentCondition
    {
        /// <summary>
        /// Id
        /// </summary>
        public int PaymentConditionId { get; set; }

        /// <summary>
        /// Name der Zahlungskondition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prozentsatz
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Wert abhängig von der Wahl der Zahlungstyps
        /// </summary>
        public int TimeValue { get; set; }

        /// <summary>
        /// Zahlungstyp
        /// </summary>
        public PayType PayType { get; set; }
    }
}