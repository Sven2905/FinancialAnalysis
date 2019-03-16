using FinancialAnalysis.Models.BaseClasses;
using Newtonsoft.Json;
using FinancialAnalysis.Models.ClientManagement;

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