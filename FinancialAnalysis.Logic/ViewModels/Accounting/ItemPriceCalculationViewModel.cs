using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Accounting;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using Formulas.PriceCalculationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ItemPriceCalculationViewModel : ViewModelBase
    {
        #region Constructor

        public ItemPriceCalculationViewModel()
        {
            SetCommands();
            SubscribeEvents();
            ItemPriceCalculationInputItem = new ItemPriceCalculationInputItem();
            ItemPriceCalculationInputItem.ValueChanged += ItemPriceCalculationInputItem_ValueChanged;
            ItemPriceCalculationOutputItem = ItemPriceCalculation.CalculatePriceForItem(ItemPriceCalculationInputItem);
        }

        #endregion Constructor

        #region Fields

        private int itemAmountPerAnno = 1000;
        private decimal hourlyWage;
        private decimal productionTime;

        #endregion Fields

        #region Methods

        private void SubscribeEvents()
        {
            MaterialOverHeadCostsCostCenters.OnAmountChanged += MaterialOverHeadCostsCostCenters_OnAmountChanged;
            ProductOverheadsCostCenters.OnAmountChanged += ProductOverheadsCostCenters_OnAmountChanged;
            AdministrativeOverheadsCostCenters.OnAmountChanged += AdministrativeOverheadsCostCenters_OnAmountChanged;
            SalesOverHeadsCostCenters.OnAmountChanged += SalesOverHeadsCostCenters_OnAmountChanged;
        }

        private void SalesOverHeadsCostCenters_OnAmountChanged(decimal amount)
        {
            Refresh();
        }

        private void AdministrativeOverheadsCostCenters_OnAmountChanged(decimal amount)
        {
            Refresh();
        }

        private void ProductOverheadsCostCenters_OnAmountChanged(decimal amount)
        {
            Refresh();
        }

        private void MaterialOverHeadCostsCostCenters_OnAmountChanged(decimal amount)
        {
            Refresh();
        }

        private void SetCommands()
        {

        }

        public void Refresh()
        {
            ItemPriceCalculationInputItem.ValueChanged -= ItemPriceCalculationInputItem_ValueChanged;

            if (ProductionTime > 0)
                ItemPriceCalculationInputItem.ProductWages = HourlyWage / ProductionTime;

            ItemPriceCalculationOutputItem = ItemPriceCalculation.CalculatePriceForItem(ItemPriceCalculationInputItem);

            if (ItemAmountPerAnno > 0 && ItemPriceCalculationInputItem.ProductionMaterial > 0)
                ItemPriceCalculationInputItem.MaterialOverheadCosts = MaterialOverHeadCostsCostCenters.Amount / (ItemAmountPerAnno * ItemPriceCalculationInputItem.ProductionMaterial);

            if (ItemAmountPerAnno > 0 && ItemPriceCalculationOutputItem.ProductWages > 0)
                ItemPriceCalculationInputItem.ProductOverheads = ProductOverheadsCostCenters.Amount / (ItemAmountPerAnno * ItemPriceCalculationOutputItem.ProductWages);

            if (ItemAmountPerAnno > 0 && ItemPriceCalculationOutputItem.ProductionCosts > 0)
                ItemPriceCalculationInputItem.AdministrativeOverheads = AdministrativeOverheadsCostCenters.Amount / (ItemAmountPerAnno * ItemPriceCalculationOutputItem.ProductionCosts);

            if (ItemAmountPerAnno > 0 && ItemPriceCalculationOutputItem.ProductionCosts > 0)
                ItemPriceCalculationInputItem.SalesOverheads = SalesOverHeadsCostCenters.Amount / (ItemAmountPerAnno * ItemPriceCalculationOutputItem.ProductionCosts);

            ItemPriceCalculationOutputItem = ItemPriceCalculation.CalculatePriceForItem(ItemPriceCalculationInputItem);

            ItemPriceCalculationInputItem.ValueChanged += ItemPriceCalculationInputItem_ValueChanged;

            RaisePropertiesChanged("ItemPriceCalculationInputItem");
            RaisePropertiesChanged("ItemPriceCalculationOutputItem");
        }

        private void ItemPriceCalculationInputItem_ValueChanged()
        {
            Refresh();
        }

        #endregion Methods

        #region Properties

        public ItemPriceCalculationInputItem ItemPriceCalculationInputItem { get; set; }
        public ItemPriceCalculationOutputItem ItemPriceCalculationOutputItem { get; set; }
        public ItemPriceCalculationItemHelper MaterialOverHeadCostsCostCenters { get; set; } = new ItemPriceCalculationItemHelper();
        public ItemPriceCalculationItemHelper ProductOverheadsCostCenters { get; set; } = new ItemPriceCalculationItemHelper();
        public ItemPriceCalculationItemHelper AdministrativeOverheadsCostCenters { get; set; } = new ItemPriceCalculationItemHelper();
        public ItemPriceCalculationItemHelper SalesOverHeadsCostCenters { get; set; } = new ItemPriceCalculationItemHelper();

        public int ItemAmountPerAnno
        {
            get { return itemAmountPerAnno; }
            set { itemAmountPerAnno = value; Refresh(); }
        }

        public decimal HourlyWage
        {
            get { return hourlyWage; }
            set { hourlyWage = value; Refresh(); }
        }

        public decimal ProductionTime
        {
            get { return productionTime; }
            set { productionTime = value; Refresh(); }
        }

        #endregion Properties

        #region Commands


        #endregion Commands
    }
}
