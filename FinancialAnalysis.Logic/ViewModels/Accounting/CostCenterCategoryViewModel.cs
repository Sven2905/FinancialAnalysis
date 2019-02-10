using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CostCenterCategoryViewModel : ViewModelBase
    {
        #region Constructor

        public CostCenterCategoryViewModel()
        {
            if (IsInDesignMode) return;

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
            var allCostCenterCategories = new SvenTechCollection<CostCenterCategory>();
            try
            {
                allCostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }

            return allCostCenterCategories;
        }

        private void NewCostCenterCategory()
        {
            SelectedCostCenterCategory = new CostCenterCategory();
            _CostCenterCategories.Add(SelectedCostCenterCategory);
        }

        private void DeleteCostCenterCategory()
        {
            if (SelectedCostCenterCategory == null) return;

            if (SelectedCostCenterCategory.CostCenterCategoryId == 0)
            {
                _CostCenterCategories.Remove(SelectedCostCenterCategory);
                SelectedCostCenterCategory = null;
                return;
            }

            try
            {
                DataContext.Instance.CostCenterCategories.Delete(SelectedCostCenterCategory.CostCenterCategoryId);
                _CostCenterCategories.Remove(SelectedCostCenterCategory);
                SelectedCostCenterCategory = null;
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void SaveCostCenterCategory()
        {
            try
            {
                if (SelectedCostCenterCategory.CostCenterCategoryId != 0)
                    DataContext.Instance.CostCenterCategories.Update(SelectedCostCenterCategory);
                else
                    SelectedCostCenterCategory.CostCenterCategoryId =
                        DataContext.Instance.CostCenterCategories.Insert(SelectedCostCenterCategory);
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private bool Validation()
        {
            if (SelectedCostCenterCategory == null) return false;
            if (string.IsNullOrEmpty(SelectedCostCenterCategory.Name)) return false;
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedCostCenterCategory == null) return;

            if (SelectedCostCenterCategory.CostCenterCategoryId == 0) SaveCostCenterCategory();

            Messenger.Default.Send(new SelectedCostCenterCategory {CostCenterCategory = SelectedCostCenterCategory});
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
                    foreach (var item in _CostCenterCategories)
                        if (item.Name.Contains(FilterText))
                            FilteredCostCenterCategories.Add(item);
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