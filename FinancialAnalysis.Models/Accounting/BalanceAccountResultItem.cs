using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class BalanceAccountResultItem : BindableBase
    {
        public int BalanceAccountId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; } = 0;
        public int ParentId { get; set; }
    }
}
