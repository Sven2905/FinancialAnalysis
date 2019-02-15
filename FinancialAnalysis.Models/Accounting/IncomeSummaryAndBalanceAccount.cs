using FinancialAnalysis.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int BalanceAccountId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; } = 0;
        public AccountType AccountType { get; set; }
    }
}
