using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Accounting;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.ProductManagement;
using Formulas.PriceCalculationMethods;
using System;
using System.Collections.Generic;
using WebApiWrapper.Accounting;
using WebApiWrapper.ProductManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ItemPriceCalculationViewModel : ViewModelBase
    {
        #region Constructor

        public ItemPriceCalculationViewModel()
        {
            CalculatePriceForOtherProducts();
            var costsPerYear = BookingCostCenterMappings.GetAllByYear(DateTime.Now.Year);

            MaterialOverHeadCostsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            ProductOverheadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            AdministrativeOverheadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            SalesOverHeadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);

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
        private Product product;
        ItemPriceCalculationItem itemPriceCalculationItem;

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
            SaveCommand = new DelegateCommand(Save, () => ItemPriceCalculationOutputItem.GrossSellingPrice != 0);
        }

        public void Refresh()
        {
            CalculatePrice();
            RaisePropertiesChanged("ItemPriceCalculationInputItem");
            RaisePropertiesChanged("ItemPriceCalculationOutputItem");
        }

        private void CalculatePrice()
        {
            ItemPriceCalculationInputItem.ValueChanged -= ItemPriceCalculationInputItem_ValueChanged;

            if (ProductionTime > 0)
                ItemPriceCalculationInputItem.ProductWages = HourlyWage * ProductionTime;

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
        }

        private void ItemPriceCalculationInputItem_ValueChanged()
        {
            Refresh();
        }

        private void Save()
        {
            SaveNewItem();

            UpdateProduct();
        }

        private void UpdateProduct()
        {
            Product.DefaultSellingPrice = ItemPriceCalculationOutputItem.GrossSellingPrice;
            Products.Update(Product);
        }

        private void SaveNewItem()
        {
            itemPriceCalculationItem = new ItemPriceCalculationItem()
            {
                AgentCommission = ItemPriceCalculationInputItem.AgentCommission,
                CustomerCashback = ItemPriceCalculationInputItem.CustomerCashback,
                CustomerDiscount = ItemPriceCalculationInputItem.CustomerDiscount,
                ProfitSurcharge = ItemPriceCalculationInputItem.ProfitSurcharge,
                Tax = ItemPriceCalculationInputItem.Tax,
                HourlyWage = HourlyWage,
                ProductionTime = ProductionTime,
                ItemAmountPerAnno = ItemAmountPerAnno,
                RefProductId = Product.ProductId
            };

            var itemPriceCalculationItemId = ItemPriceCalculationItems.Insert(itemPriceCalculationItem);

            var ItemPriceCalculationItemCostCenterList = new List<ItemPriceCalculationItemCostCenter>();

            foreach (var item in MaterialOverHeadCostsCostCenters.CostCenterFlatStructures)
            {
                if (item.IsActive && item.CostCenter != null)
                {
                    ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter = new ItemPriceCalculationItemCostCenter()
                    {
                        RefItemPriceCalculationItemId = itemPriceCalculationItemId,
                        RefCostCenterId = item.CostCenter.CostCenterId,
                        ItemPriceCalculationItemCostCenterType = ItemPriceCalculationItemCostCenterType.MaterialOverheadCosts
                    };
                    ItemPriceCalculationItemCostCenterList.Add(ItemPriceCalculationItemCostCenter);
                }
            }

            foreach (var item in ProductOverheadsCostCenters.CostCenterFlatStructures)
            {
                if (item.IsActive && item.CostCenter != null)
                {
                    ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter = new ItemPriceCalculationItemCostCenter()
                    {
                        RefItemPriceCalculationItemId = itemPriceCalculationItemId,
                        RefCostCenterId = item.CostCenter.CostCenterId,
                        ItemPriceCalculationItemCostCenterType = ItemPriceCalculationItemCostCenterType.ProductOverheadCosts
                    };
                    ItemPriceCalculationItemCostCenterList.Add(ItemPriceCalculationItemCostCenter);
                }
            }

            foreach (var item in AdministrativeOverheadsCostCenters.CostCenterFlatStructures)
            {
                if (item.IsActive && item.CostCenter != null)
                {
                    ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter = new ItemPriceCalculationItemCostCenter()
                    {
                        RefItemPriceCalculationItemId = itemPriceCalculationItemId,
                        RefCostCenterId = item.CostCenter.CostCenterId,
                        ItemPriceCalculationItemCostCenterType = ItemPriceCalculationItemCostCenterType.AdministrativeOverheadCosts
                    };
                    ItemPriceCalculationItemCostCenterList.Add(ItemPriceCalculationItemCostCenter);
                }
            }

            foreach (var item in SalesOverHeadsCostCenters.CostCenterFlatStructures)
            {
                if (item.IsActive && item.CostCenter != null)
                {
                    ItemPriceCalculationItemCostCenter ItemPriceCalculationItemCostCenter = new ItemPriceCalculationItemCostCenter()
                    {
                        RefItemPriceCalculationItemId = itemPriceCalculationItemId,
                        RefCostCenterId = item.CostCenter.CostCenterId,
                        ItemPriceCalculationItemCostCenterType = ItemPriceCalculationItemCostCenterType.SalesOverheadCosts
                    };
                    ItemPriceCalculationItemCostCenterList.Add(ItemPriceCalculationItemCostCenter);
                }
            }

            ItemPriceCalculationItemCostCenters.Insert(ItemPriceCalculationItemCostCenterList);
        }

        private void CalculateOthers()
        {
            foreach (var item in MaterialOverHeadCostsCostCenters.CostCenterFlatStructures)
            {
                if (item.CostCenter == null)
                    continue;

                var itemPriceCalculationItemList = ItemPriceCalculationItems.GetByCostCenter(item.CostCenter.CostCenterId);

                foreach (ItemPriceCalculationItem tmpItemPriceCalculationItem in itemPriceCalculationItemList)
                {
                    if (tmpItemPriceCalculationItem.ItemPriceCalculationItemId != itemPriceCalculationItem.ItemPriceCalculationItemId)
                    {

                    }
                }
            }

        }

        #endregion Methods

        #region Properties

        public ItemPriceCalculationInputItem ItemPriceCalculationInputItem { get; set; }
        public ItemPriceCalculationOutputItem ItemPriceCalculationOutputItem { get; set; }
        public ItemPriceCalculationItemHelper MaterialOverHeadCostsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper ProductOverheadsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper AdministrativeOverheadsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper SalesOverHeadsCostCenters { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public Product Product { get; set; }

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
    }
}