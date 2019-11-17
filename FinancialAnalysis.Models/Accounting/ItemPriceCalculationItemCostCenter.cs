using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ItemPriceCalculationItemCostCenter : BaseClass
    {
        public int ItemPriceCalculationItemCostCenterId { get; set; }
        public int RefCostCenterId { get; set; }
        public int RefItemPriceCalculationItemId { get; set; }
        public ItemPriceCalculationItemCostCenterType ItemPriceCalculationItemCostCenterType { get; set; }
    }
}
