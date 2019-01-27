using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductCategoryViewModel : ViewModelBase
    {
        #region Fields

        private readonly ProductCategory _SelectedProductCategory;
        private SvenTechCollection<ProductCategory> _ProductCategories = new SvenTechCollection<ProductCategory>();
        private string _FilterText = string.Empty;

        #endregion Fields

        #region Constructor

        public ProductCategoryViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            FilteredProductCategories = _ProductCategories = LoadAllProductCategories();
            NewProductCategoryCommand = new DelegateCommand(NewProductCategory);
            SaveProductCategoryCommand = new DelegateCommand(SaveProductCategory, () => Validation());
            DeleteProductCategoryCommand = new DelegateCommand(DeleteProductCategory, () => (SelectedProductCategory != null));
            SelectedCommand = new DelegateCommand(() =>
            {
                SendSelectedToParent();
                CloseAction();
            });
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<ProductCategory> LoadAllProductCategories()
        {
            SvenTechCollection<ProductCategory> allProductCategories = new SvenTechCollection<ProductCategory>();
            try
            {
                allProductCategories = DataLayer.Instance.ProductCategories.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
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
            if (SelectedProductCategory == null)
            {
                return;
            }

            if (SelectedProductCategory.ProductCategoryId == 0)
            {
                _ProductCategories.Remove(SelectedProductCategory);
                SelectedProductCategory = null;
                return;
            }

            try
            {
                DataLayer.Instance.ProductCategories.Delete(SelectedProductCategory.ProductCategoryId);
                _ProductCategories.Remove(SelectedProductCategory);
                SelectedProductCategory = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveProductCategory()
        {
            try
            {
                if (SelectedProductCategory.ProductCategoryId != 0)
                {
                    DataLayer.Instance.ProductCategories.Update(SelectedProductCategory);
                }
                else
                {
                    SelectedProductCategory.ProductCategoryId = DataLayer.Instance.ProductCategories.Insert(SelectedProductCategory);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedProductCategory == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedProductCategory.Name))
            {
                return false;
            }
            return true;
        }

        public void SendSelectedToParent()
        {
            if (SelectedProductCategory == null)
            {
                return;
            }

            if (SelectedProductCategory.ProductCategoryId == 0)
            {
                SaveProductCategory();
            }

            Messenger.Default.Send(new SelectedProductCategory { ProductCategory = SelectedProductCategory });
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<ProductCategory> FilteredProductCategories { get; set; } = new SvenTechCollection<ProductCategory>();
        public DelegateCommand NewProductCategoryCommand { get; set; }
        public DelegateCommand SaveProductCategoryCommand { get; set; }
        public DelegateCommand DeleteProductCategoryCommand { get; set; }
        public DelegateCommand SelectedCommand { get; }

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredProductCategories = new SvenTechCollection<ProductCategory>();
                    foreach (var item in _ProductCategories)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredProductCategories.Add(item);
                        }
                    }
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
        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
