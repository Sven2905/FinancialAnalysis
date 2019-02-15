using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
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

        public int GainAndLossAccountId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
}
