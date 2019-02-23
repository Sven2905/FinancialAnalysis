using FinancialAnalysis.Models.Enums;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Bilanz-/GuV-Posten
    /// </summary>
    public class BalanceAccount
    {
        public BalanceAccount()
        {
        }

        public BalanceAccount(int BalanceAccountId, string Name, AccountType AccountType, int ParentId = 0)
        {
            this.BalanceAccountId = BalanceAccountId;
            this.Name = Name;
            this.ParentId = ParentId;
            this.AccountType = AccountType;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int BalanceAccountId { get; set; }

        /// <summary>
        /// Name des Postens
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id des übergeordneten Postens
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// Aktiv- oder Passivkonto
        /// </summary>
        public AccountType AccountType { get; set; }
    }
}