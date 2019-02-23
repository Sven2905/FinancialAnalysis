namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Gewinn- und Verlustrechnungskonto
    /// </summary>
    public class GainAndLossAccount
    {
        public GainAndLossAccount()
        {
        }

        public GainAndLossAccount(int GainAndLossAccountId, string Name, int ParentId = 0)
        {
            this.GainAndLossAccountId = GainAndLossAccountId;
            this.Name = Name;
            this.ParentId = ParentId;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int GainAndLossAccountId { get; set; }

        /// <summary>
        /// Name des Kontos
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Übergeordnetes Konto
        /// </summary>
        public int ParentId { get; set; }
    }
}