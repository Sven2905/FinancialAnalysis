using DevExpress.Mvvm;
using Formulas.PriceCalculationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ItemPriceCalculationViewModel : ViewModelBase
    {
        public ItemPriceCalculationViewModel()
        {
            ItemPriceCalculationInputItem = new ItemPriceCalculationInputItem();
            ItemPriceCalculationInputItem.ValueChanged += ItemPriceCalculationInputItem_ValueChanged;
            ItemPriceCalculationOutputItem = ItemPriceCalculation.CalculatePriceForItem(ItemPriceCalculationInputItem);
        }

        private void ItemPriceCalculationInputItem_ValueChanged()
        {
            ItemPriceCalculationOutputItem = ItemPriceCalculation.CalculatePriceForItem(ItemPriceCalculationInputItem);
        }

        public ItemPriceCalculationInputItem ItemPriceCalculationInputItem { get; set; }
        public ItemPriceCalculationOutputItem ItemPriceCalculationOutputItem { get; set; }
    }
}
