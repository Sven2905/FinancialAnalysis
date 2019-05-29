using FinancialAnalysis.Models.BaseClasses;
using FinancialAnalysis.Models.ClientManagement;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Debitor
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
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