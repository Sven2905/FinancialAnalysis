﻿using DevExpress.Mvvm;
using DevExpress.Xpf.Bars;
using FinancialAnalysis.Logic.Accounting;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.ProductManagement;
using Formulas.PriceCalculationMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApiWrapper.Accounting;
using WebApiWrapper.ProductManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ItemPriceCalculationViewModel : ViewModelBase
    {
        #region Constructor

        public ItemPriceCalculationViewModel()
        {
            var costsPerYear = BookingCostCenterMappings.GetAllByYear(DateTime.Now.Year);

            MaterialOverHeadCostsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            ProductOverheadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            AdministrativeOverheadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            SalesOverHeadsCostCenters = new ItemPriceCalculationItemHelper(costsPerYear);
            SetCommands();
            SubscribeEvents();
            StandardItemPriceCalculation = new StandardItemPriceCalculation();
            StandardItemPriceCalculation.ValueChanged += ItemPriceCalculationInputItem_ValueChanged;
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

        private void LoadData()
        {
            var itemPriceCalculationItem = ItemPriceCalculationItems.GetByRefProductId(Product.ProductId);

            if (itemPriceCalculationItem == null)
            {
                StandardItemPriceCalculation = new StandardItemPriceCalculation();
                return;
            }

            StandardItemPriceCalculation.AgentCommission = itemPriceCalculationItem.AgentCommission;
            StandardItemPriceCalculation.CustomerCashback = itemPriceCalculationItem.CustomerCashback;
            StandardItemPriceCalculation.CustomerDiscount = itemPriceCalculationItem.CustomerDiscount;
            StandardItemPriceCalculation.ProfitSurcharge = itemPriceCalculationItem.ProfitSurcharge;
            StandardItemPriceCalculation.Tax = itemPriceCalculationItem.Tax;
            StandardItemPriceCalculation.HourlyWage = itemPriceCalculationItem.HourlyWage;
            StandardItemPriceCalculation.ProductionTime = itemPriceCalculationItem.ProductionTime;
            StandardItemPriceCalculation.ItemAmountPerAnno = itemPriceCalculationItem.ItemAmountPerAnno;

            foreach (var item in itemPriceCalculationItem.ItemPriceCalculationItemCostCenters)
            {
                switch (item.ItemPriceCalculationItemCostCenterType)
                {
                    case ItemPriceCalculationItemCostCenterType.MaterialOverheadCosts:
                        MaterialOverHeadCostsCostCenters.CostCenterFlatStructures.SingleOrDefault(x => x.CostCenter?.CostCenterId == item.RefCostCenterId).IsActive = true;
                        break;
                    case ItemPriceCalculationItemCostCenterType.ProductOverheadCosts:
                        ProductOverheadsCostCenters.CostCenterFlatStructures.SingleOrDefault(x => x.CostCenter?.CostCenterId == item.RefCostCenterId).IsActive = true;
                        break;
                    case ItemPriceCalculationItemCostCenterType.AdministrativeOverheadCosts:
                        AdministrativeOverheadsCostCenters.CostCenterFlatStructures.SingleOrDefault(x => x.CostCenter?.CostCenterId == item.RefCostCenterId).IsActive = true;
                        break;
                    case ItemPriceCalculationItemCostCenterType.SalesOverheadCosts:
                        SalesOverHeadsCostCenters.CostCenterFlatStructures.SingleOrDefault(x => x.CostCenter?.CostCenterId == item.RefCostCenterId).IsActive = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SubscribeEvents()
        {
            MaterialOverHeadCostsCostCenters.OnAmountChanged += CostCenters_OnAmountChanged;
            ProductOverheadsCostCenters.OnAmountChanged += CostCenters_OnAmountChanged;
            AdministrativeOverheadsCostCenters.OnAmountChanged += CostCenters_OnAmountChanged;
            SalesOverHeadsCostCenters.OnAmountChanged += CostCenters_OnAmountChanged;
        }

        private void CostCenters_OnAmountChanged()
        {
            Refresh();
        }

        private void SetCommands()
        {
            SaveCommand = new DelegateCommand(Save, () => StandardItemPriceCalculation.GrossSellingPrice != 0);
        }

        public void Refresh()
        {
            MaterialOverHeadCostsCostCenters.ItemAmountPerAnno = StandardItemPriceCalculation.ItemAmountPerAnno;
            ProductOverheadsCostCenters.ItemAmountPerAnno = StandardItemPriceCalculation.ItemAmountPerAnno;
            AdministrativeOverheadsCostCenters.ItemAmountPerAnno = StandardItemPriceCalculation.ItemAmountPerAnno;
            SalesOverHeadsCostCenters.ItemAmountPerAnno = StandardItemPriceCalculation.ItemAmountPerAnno;
            StandardItemPriceCalculation.MaterialOverHeadCostCentersAmount = MaterialOverHeadCostsCostCenters.Amount;
            StandardItemPriceCalculation.ProductOverHeadCostCentersAmount = ProductOverheadsCostCenters.Amount;
            StandardItemPriceCalculation.AdministrativeOverHeadCostCentersAmount = AdministrativeOverheadsCostCenters.Amount;
            StandardItemPriceCalculation.SalesOverHeadCostCentersAmount = SalesOverHeadsCostCenters.Amount;
            RaisePropertiesChanged("StandardItemPriceCalculation");
        }


        private void ItemPriceCalculationInputItem_ValueChanged()
        {
            Refresh();
        }

        private void Save()
        {
            var item = ItemPriceCalculationItems.GetByRefProductId(Product.ProductId);
            if (item != null)
            {
                UpdateSavedItem(item);
                SaveCostCenters(item.ItemPriceCalculationItemId);
            }
            else
            {
                SaveNewItem();
            }

            UpdateProduct();
        }

        private void UpdateProduct()
        {
            Product.DefaultSellingPrice = StandardItemPriceCalculation.GrossSellingPrice;
            Products.Update(Product);
        }

        private void SaveNewItem()
        {
            itemPriceCalculationItem = new ItemPriceCalculationItem()
            {
                AgentCommission = StandardItemPriceCalculation.AgentCommission,
                CustomerCashback = StandardItemPriceCalculation.CustomerCashback,
                CustomerDiscount = StandardItemPriceCalculation.CustomerDiscount,
                ProfitSurcharge = StandardItemPriceCalculation.ProfitSurcharge,
                Tax = StandardItemPriceCalculation.Tax,
                HourlyWage = HourlyWage,
                ProductionTime = ProductionTime,
                ItemAmountPerAnno = ItemAmountPerAnno,
                RefProductId = Product.ProductId
            };

            var itemPriceCalculationItemId = ItemPriceCalculationItems.Insert(itemPriceCalculationItem);

            SaveCostCenters(itemPriceCalculationItemId);
        }

        private void UpdateSavedItem(ItemPriceCalculationItem item)
        {
            item.AgentCommission = StandardItemPriceCalculation.AgentCommission;
            item.CustomerCashback = StandardItemPriceCalculation.CustomerCashback;
            item.CustomerDiscount = StandardItemPriceCalculation.CustomerDiscount;
            item.ProfitSurcharge = StandardItemPriceCalculation.ProfitSurcharge;
            item.Tax = StandardItemPriceCalculation.Tax;
            item.HourlyWage = HourlyWage;
            item.ProductionTime = ProductionTime;
            item.ItemAmountPerAnno = ItemAmountPerAnno;
            item.RefProductId = Product.ProductId;

            ItemPriceCalculationItems.Update(item);
        }

        private void SaveCostCenters(int itemPriceCalculationItemId)
        {
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

            ItemPriceCalculationItemCostCenters.DeleteByRefItemPriceCalculationItemId(itemPriceCalculationItemId);
            ItemPriceCalculationItemCostCenters.Insert(ItemPriceCalculationItemCostCenterList);
            CalculateOthers();
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
                        ItemPriceCalculationItems.Update(tmpItemPriceCalculationItem);
                    }
                }
            }

        }

        #endregion Methods

        #region Properties

        public StandardItemPriceCalculation StandardItemPriceCalculation { get; set; }
        public ItemPriceCalculationItemHelper MaterialOverHeadCostsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper ProductOverheadsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper AdministrativeOverheadsCostCenters { get; set; }
        public ItemPriceCalculationItemHelper SalesOverHeadsCostCenters { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
                if (product != null && product.ProductId > 0)
                    LoadData();
            }
        }

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