using FinancialAnalysis.Models.BaseClasses;
using FinancialAnalysis.Models.ClientManagement;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Debitor
    /// </summary>
    public class Debitor : CreditorDebitorBase
    {
        public Debitor()
        {
            Client = new Client();
            CostAccount = new CostAccount();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int DebitorId { get; set; }
    }
}