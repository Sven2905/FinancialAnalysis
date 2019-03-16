using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SelectedCostAccount
    {
        public CostAccount CostAccount { get; set; }
        public AccountingType AccountingType { get; set; }
    }
}