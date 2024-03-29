﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using System;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostCenterCategoryViewModel : ViewModelBase
    {
        #region Constructor

        public CostCenterCategoryViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            FilteredCostCenterCategories = _CostCenterCategories = LoadAllCostCenterCategories();
            NewCostCenterCategoryCommand = new DelegateCommand(NewCostCenterCategory);
            SaveCostCenterCategoryCommand = new DelegateCommand(SaveCostCenterCategory, () => Validation());
            DeleteCostCenterCategoryCommand =
                new DelegateCommand(DeleteCostCenterCategory, () => SelectedCostCenterCategory != null);
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Fields

        private readonly CostCenterCategory _SelectedCostCenterCategory;

        private readonly SvenTechCollection<CostCenterCategory> _CostCenterCategories =
            new SvenTechCollection<CostCenterCategory>();

        private string _FilterText = string.Empty;

        #endregion Fields

        #region Methods

        private SvenTechCollection<CostCenterCategory> LoadAllCostCenterCategories()
        {
            SvenTechCollection<CostCenterCategory> allCostCenterCategories = new SvenTechCollection<CostCenterCategory>();
            return CostCenterCategories.GetAll().ToSvenTechCollection();
        }

        private void NewCostCenterCategory()
        {
            SelectedCostCenterCategory = new CostCenterCategory();
            _CostCenterCategories.Add(SelectedCostCenterCategory);
        }

        private void DeleteCostCenterCategory()
        {
            if (SelectedCostCenterCategory == null)
            {
                return;
            }

            if (SelectedCostCenterCategory.CostCenterCategoryId == 0)
            {
                _CostCenterCategories.Remove(SelectedCostCenterCategory);
                SelectedCostCenterCategory = null;
                return;
            }

            CostCenterCategories.Delete(SelectedCostCenterCategory.CostCenterCategoryId);
            _CostCenterCategories.Remove(SelectedCostCenterCategory);
            SelectedCostCenterCategory = null;
        }

        private void SaveCostCenterCategory()
        {
            if (SelectedCostCenterCategory.CostCenterCategoryId != 0)
            {
                CostCenterCategories.Update(SelectedCostCenterCategory);
            }
            else
            {
                SelectedCostCenterCategory.CostCenterCategoryId =
                    CostCenterCategories.Insert(SelectedCostCenterCategory);
            }
        }

        private bool Validation()
        {
            if (SelectedCostCenterCategory == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SelectedCostCenterCategory.Name))
            {
                return false;
            }

            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedCostCenterCategory == null)
            {
                return;
            }

            if (SelectedCostCenterCategory.CostCenterCategoryId == 0)
            {
                SaveCostCenterCategory();
            }

            Messenger.Default.Send(new SelectedCostCenterCategory { CostCenterCategory = SelectedCostCenterCategory });
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<CostCenterCategory> FilteredCostCenterCategories { get; set; } =
            new SvenTechCollection<CostCenterCategory>();

        public DelegateCommand NewCostCenterCategoryCommand { get; set; }
        public DelegateCommand SaveCostCenterCategoryCommand { get; set; }
        public DelegateCommand DeleteCostCenterCategoryCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredCostCenterCategories = new SvenTechCollection<CostCenterCategory>();
                    foreach (CostCenterCategory item in _CostCenterCategories)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredCostCenterCategories.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredCostCenterCategories = _CostCenterCategories;
                    RaisePropertiesChanged("FilteredCostCenterCategories");
                }
            }
        }

        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public Action CloseAction { get; set; }
        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}