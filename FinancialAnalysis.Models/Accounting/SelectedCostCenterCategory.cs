using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class SelectedCostCenterCategory
    {
        public CostCenterCategory CostCenterCategory { get; set; }
    }
}