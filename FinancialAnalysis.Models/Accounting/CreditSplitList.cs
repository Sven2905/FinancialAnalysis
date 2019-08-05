using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class CreditSplitList
    {
        public IEnumerable<Credit> Credits { get; set; }
    }
}