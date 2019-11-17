using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BalanceAccountResultItem : BaseClass
    {
        public int BalanceAccountId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; } = 0;
        public int ParentId { get; set; }
    }
}