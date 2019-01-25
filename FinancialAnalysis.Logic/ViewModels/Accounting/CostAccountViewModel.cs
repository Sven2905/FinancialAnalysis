﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostAccountViewModel : ViewModelBase
    {
        #region Constructur

        public CostAccountViewModel()
        {
            if (IsInDesignMode)
                return;

            RefreshLists();
        }

        #endregion Constructor

        #region Fields

        private SvenTechCollection<CostAccount> _CostAccounts = new SvenTechCollection<CostAccount>();
        private CostAccountCategory _SelectedCategory;
        private CostAccount _SelectedCostAccount;

        #endregion Fields

        #region Methods

        public void RefreshLists()
        {
            try
            {
                CostAccountCategories = DataLayer.Instance.CostAccountCategories.GetAll().ToSvenTechCollection();
                CostAccountCategoriesHierachical = CostAccountCategories.ToHierachicalCollection<CostAccountCategory>().ToSvenTechCollection();
                TaxTypes = DataLayer.Instance.TaxTypes.GetAll().ToSvenTechCollection();
                _CostAccounts = DataLayer.Instance.CostAccounts.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void FilterCostAccounts()
        {
            FilteredCostAccounts.Clear();
            if (SelectedCategory.IsNull()) return;

            var ids = GetChildIds(SelectedCategory.CostAccountCategoryId).ToList();
            if (ids.IsNull()) return;

            ids.Add(SelectedCategory.CostAccountCategoryId);
            FilteredCostAccounts.AddRange(_CostAccounts.Where(x => ids.Contains(x.RefCostAccountCategoryId)));
        }

        private IEnumerable<int> GetChildIds(int motherId)
        {
            var result = new List<int>();
            var ids = CostAccountCategories.Where(x => x.ParentCategoryId == motherId)
                .Select(x => x.CostAccountCategoryId);
            result.AddRange(ids);
            if (ids.Any())
                foreach (var id in ids)
                    result.AddRange(GetChildIds(id));
            return result;
        }

        #endregion Methods

        #region Properties

        public CostAccount SelectedCostAccount
        {
            get => _SelectedCostAccount;
            set
            {
                if (_SelectedCostAccount != null)
                    _SelectedCostAccount.PropertyChanged -= _SelectedCostAccount_PropertyChanged;

                _SelectedCostAccount = value;
                if (_SelectedCostAccount != null)
                    _SelectedCostAccount.PropertyChanged += _SelectedCostAccount_PropertyChanged;
            }
        }

        private void _SelectedCostAccount_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DataLayer.Instance.CostAccounts.Update(SelectedCostAccount);
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategoriesHierachical { get; set; }
        public SvenTechCollection<TaxType> TaxTypes { get; set; }

        public SvenTechCollection<CostAccount> FilteredCostAccounts { get; set; } =
            new SvenTechCollection<CostAccount>();

        public CostAccountCategory SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                FilterCostAccounts();
            }
        }

        public SvenTechCollection<CostAccountCategory> CostAccountCategories { get; set; } =
            new SvenTechCollection<CostAccountCategory>();

        #endregion Properties
    }
}