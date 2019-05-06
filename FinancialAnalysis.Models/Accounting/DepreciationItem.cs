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
    public class DepreciationItem : BindableBase
    {
        public int DepreciationItemId { get; set; }
        public string Name { get; set; }
        public int Years { get; set; }
        public int StartYear { get; set; }
        public decimal InitialValue { get; set; }
        public decimal AssetValue { get; set; }
        public DepreciationType DepreciationType { get; set; }
        public bool IsDepreciated { get; set; }
    }
}
