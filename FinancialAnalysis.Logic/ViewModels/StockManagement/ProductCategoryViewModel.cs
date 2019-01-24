﻿using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductCategoryViewModel : ViewModelBase
    {
        #region Fields

        private readonly ProductCategory _SelectedProductCategory;
        private readonly BitmapImage _Image;
        private SvenTechCollection<ProductCategory> _ProductCategories = new SvenTechCollection<ProductCategory>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public ProductCategoryViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _ProductCategories = LoadAllProductCategories();
            NewProductCategoryCommand = new DelegateCommand(NewProductCategory);
            SaveProductCategoryCommand = new DelegateCommand(SaveProductCategory, () => Validation());
            DeleteProductCategoryCommand = new DelegateCommand(DeleteProductCategory, () => (SelectedProductCategory != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<ProductCategory> LoadAllProductCategories()
        {
            SvenTechCollection<ProductCategory> allProductCategories = new SvenTechCollection<ProductCategory>();
            try
            {
                using (var db = new DataLayer())
                {
                    allProductCategories = db.ProductCategories.GetAll().ToSvenTechCollection();
                }
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
                using (var db = new DataLayer())
                {
                    db.ProductCategories.Delete(SelectedProductCategory.ProductCategoryId);
                    _ProductCategories.Remove(SelectedProductCategory);
                    SelectedProductCategory = null;
                }
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
                    using (var db = new DataLayer())
                    {
                        db.ProductCategories.Update(SelectedProductCategory);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.ProductCategories.Insert(SelectedProductCategory);
                    }
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

        #endregion Methods

        #region Properties

        public SvenTechCollection<ProductCategory> FilteredProductCategories { get; set; } = new SvenTechCollection<ProductCategory>();
        public DelegateCommand NewProductCategoryCommand { get; set; }
        public DelegateCommand SaveProductCategoryCommand { get; set; }
        public DelegateCommand DeleteProductCategoryCommand { get; set; }
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
                }
            }
        }
        public ProductCategory SelectedProductCategory { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
