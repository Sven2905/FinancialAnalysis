using FinancialAnalysis.Models.BaseClasses;
using FinancialAnalysis.Models.ClientManagement;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Kreditor
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Creditor : CreditorDebitorBase
    {
        public Creditor()
        {
            Client = new Client();
            CostAccount = new CostAccount();
        }

        /// <summary>
        /// Id
        /// </summary>
        public int CreditorId { get; set; }
    }
}