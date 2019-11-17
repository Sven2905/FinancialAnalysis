using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    [JsonObject(MemberSerialization.OptOut)]
    public class ItemPriceCalculationItem : BaseClass
    {
        public int ItemPriceCalculationItemId { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal ProductionTime { get; set; }
        public int ItemAmountPerAnno { get; set; }
        public int RefProductId { get; set; }
        public decimal ProfitSurcharge { get; set; }
        public decimal CustomerCashback { get; set; }
        public decimal AgentCommission { get; set; }
        public decimal CustomerDiscount { get; set; }
        public decimal Tax { get; set; }
        public List<ItemPriceCalculationItemCostCenter> ItemPriceCalculationItemCostCenters { get; set; } = new List<ItemPriceCalculationItemCostCenter>();
    }
}
