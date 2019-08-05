﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Accounting.CostCenterManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.Accounting
{
    public class ItemPriceCalculationItemHelper : ViewModelBase
    {
        #region Events

        public delegate void AmountChangedEvent(decimal amount);
        public event AmountChangedEvent OnAmountChanged;

        #endregion

        #region Properties

        public ItemPriceCalculationItemHelper()
        {
            LoadCostCenters();
        }
        public SvenTechCollection<CostCenter> CostCenterList { get; set; } = new SvenTechCollection<CostCenter>();
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; } = new SvenTechCollection<CostCenterCategory>();
        public SvenTechCollection<CostCenterFlatStructure> CostCenterFlatStructures { get; set; } = new SvenTechCollection<CostCenterFlatStructure>();
        public decimal Amount => CalculateAmount();

        #endregion

        #region Methods

        private decimal CalculateAmount()
        {
            decimal amount = 0;
            foreach (var item in CostCenterFlatStructures)
            {
                if (item.IsActive)
                {
                    if (item.CostCenter != null)
                    {
                        amount += CostCenterBudgets.GetAnnuallyCosts(item.CostCenter.CostCenterId, DateTime.Now.Year).Sum(x => x.Amount);
                    }
                }
            }
            return amount;
        }
        private void LoadCostCenters()
        {
            CostCenterList = CostCenters.GetAll().ToSvenTechCollection();
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            SetupFlatStructure();
        }
        private void CostCenterFlatStructures_OnItemPropertyChanged(object sender, object item, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CostCenterFlatStructures.OnItemPropertyChanged -=
                CostCenterFlatStructures_OnItemPropertyChanged;

            CostCenterFlatStructure costCenter = (CostCenterFlatStructure)item;

            foreach (CostCenterFlatStructure CostCenterFlatStructure in CostCenterFlatStructures)
            {
                if (CostCenterFlatStructure.ParentKey == costCenter.Key && costCenter.IsActive)
                {
                    CostCenterFlatStructure.IsActive = true;
                }
                else if (CostCenterFlatStructure.ParentKey == costCenter.Key && costCenter.IsActive == false)
                {
                    CostCenterFlatStructure.IsActive = false;
                }
            }

            if (costCenter.IsActive)
            {
                int parentKey = costCenter.ParentKey;
                do
                {
                    CostCenterFlatStructure parentCategory =
                        CostCenterFlatStructures.SingleOrDefault(x => x.Key == parentKey);
                    if (parentCategory != null)
                    {
                        parentCategory.IsActive = true;
                        parentKey = parentCategory.ParentKey;
                    }
                    else
                    {
                        parentKey = 0;
                    }
                } while (parentKey != 0);
            }

            CostCenterFlatStructures.OnItemPropertyChanged +=
                CostCenterFlatStructures_OnItemPropertyChanged;
            RaisePropertyChanged("CostCenterFlatStructures");
            OnAmountChanged?.Invoke(Amount);
        }
        private void SetupFlatStructure()
        {
            int key = 0;
            CostCenterFlatStructures = new SvenTechCollection<CostCenterFlatStructure>();
            CostCenterFlatStructures.OnItemPropertyChanged += CostCenterFlatStructures_OnItemPropertyChanged;
            foreach (CostCenterCategory item in CostCenterCategoryList)
            {
                CostCenterFlatStructures.Add(new CostCenterFlatStructure()
                {
                    CostCenterCategory = item,
                    Key = key = item.CostCenterCategoryId,
                    ParentKey = 0
                });
                key++;
            }

            foreach (CostCenter item in CostCenterList)
            {
                CostCenterFlatStructures.Add(new CostCenterFlatStructure()
                {
                    CostCenter = item,
                    Key = key,
                    ParentKey = item.RefCostCenterCategoryId
                });
                key++;
            }
        }

        #endregion
    }
}
