using Newtonsoft.Json;
using System.Collections.Generic;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class DebitSplitList
    {
        public IEnumerable<Debit> Debits { get; set; }
    }
}