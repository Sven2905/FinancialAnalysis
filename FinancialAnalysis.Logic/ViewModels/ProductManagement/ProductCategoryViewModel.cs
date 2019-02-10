using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductCategoryViewModel : ViewModelBase
    {
        #region Constructor

        public ProductCategoryViewModel()
        {
            if (IsInDesignMode) return;

            FilteredProductCategories = _ProductCategories = LoadAllProductCategories();
            NewProductCategoryCommand = new DelegateCommand(NewProductCategory);
            SaveProductCategoryCommand = new DelegateCommand(SaveProductCategory, () => Validation());
            DeleteProductCategoryCommand =
                new DelegateCommand(DeleteProductCategory, () => SelectedProductCategory != null);
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Fields

        private readonly ProductCategory _SelectedProductCategory;

        private readonly SvenTechCollection<ProductCategory> _ProductCategories =
            new SvenTechCollection<ProductCategory>();

        private string _FilterText = string.Empty;

        #endregion Fields

        #region Methods

        private SvenTechCollection<ProductCategory> LoadAllProductCategories()
        {
            var allProductCategories = new SvenTechCollection<ProductCategory>();
            try
            {
                allProductCategories = DataContext.Instance.ProductCategories.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }

            return allProductCategories;
        }

        private void NewProductCategory()
        {
            SelectedProductCategory = new ProductCategory();
            _ProductCategories.Add(SelectedProductCategory);
        }

        private void DeleteProductCategory()
        {
            if (SelectedProductCategory == null) return;

            if (SelectedProductCategory.ProductCategoryId == 0)
            {
                _ProductCategories.Remove(SelectedProductCategory);
                SelectedProductCategory = null;
                return;
            }

            try
            {
                DataContext.Instance.ProductCategories.Delete(SelectedProductCategory.ProductCategoryId);
                _ProductCategories.Remove(SelectedProductCategory);
                SelectedProductCategory = null;
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void SaveProductCategory()
        {
            try
            {
                if (SelectedProductCategory.ProductCategoryId != 0)
                    DataContext.Instance.ProductCategories.Update(SelectedProductCategory);
                else
                    SelectedProductCategory.ProductCategoryId =
                        DataContext.Instance.ProductCategories.Insert(SelectedProductCategory);
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private bool Validation()
        {
            if (SelectedProductCategory == null) return false;
            if (string.IsNullOrEmpty(SelectedProductCategory.Name)) return false;
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedProductCategory == null) return;

            if (SelectedProductCategory.ProductCategoryId == 0) SaveProductCategory();

            Messenger.Default.Send(new SelectedProductCategory {ProductCategory = SelectedProductCategory});
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<ProductCategory> FilteredProductCategories { get; set; } =
            new SvenTechCollection<ProductCategory>();

        public DelegateCommand NewProductCategoryCommand { get; set; }
        public DelegateCommand SaveProductCategoryCommand { get; set; }
        public DelegateCommand DeleteProductCategoryCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredProductCategories = new SvenTechCollection<ProductCategory>();
                    foreach (var item in _ProductCategories)
                        if (item.Name.Contains(FilterText))
                            FilteredProductCategories.Add(item);
                }
                else
                {
                    FilteredProductCategories = _ProductCategories;
                    RaisePropertiesChanged("FilteredProductCategories");
                }
            }
        }

        public ProductCategory SelectedProductCategory { get; set; }
        public Action CloseAction { get; set; }
        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}