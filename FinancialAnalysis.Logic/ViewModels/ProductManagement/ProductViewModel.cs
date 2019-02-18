using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        #region Constructor

        public ProductViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            Messenger.Default.Register<SelectedProductCategory>(this, ChangeSelectedProductCategory);

            Task.Run(() => GetData());

            NewProductCommand = new DelegateCommand(NewProduct);
            SaveProductCommand = new DelegateCommand(SaveProduct, () => Validation());
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => SelectedProduct != null);
            OpenProductCategoriesWindowCommand = new DelegateCommand(OpenProductCategoriesWindow);
        }

        #endregion Constructor

        #region UserRights

        public bool AllowProductCategories =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessProductCategories) ||
            Globals.ActiveUser.IsAdministrator;

        #endregion UserRights

        #region Fields

        private Product _SelectedProduct;
        private BitmapImage _Image;
        private SvenTechCollection<Product> _Products = new SvenTechCollection<Product>();
        private string _FilterText;

        #endregion Fields

        #region Methods

        private void GetData()
        {
            FilteredProducts = _Products = LoadAllProducts();
            ProductCategories = LoadAllProductCategories();
        }

        private void OpenProductCategoriesWindow()
        {
            Messenger.Default.Send(new OpenProductCategoriesWindowMessage());
        }

        private SvenTechCollection<Product> LoadAllProducts()
        {
            var allProducts = new SvenTechCollection<Product>();
            return DataContext.Instance.Products.GetAll().ToSvenTechCollection();
        }

        private SvenTechCollection<ProductCategory> LoadAllProductCategories()
        {
            var allProductCategories = new SvenTechCollection<ProductCategory>();
            return DataContext.Instance.ProductCategories.GetAll().ToSvenTechCollection();
        }

        private void NewProduct()
        {
            SelectedProduct = new Product();
            _Products.Add(SelectedProduct);
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            if (SelectedProduct.ProductId == 0)
            {
                _Products.Remove(SelectedProduct);
                SelectedProduct = null;
                return;
            }

            DataContext.Instance.Products.Delete(SelectedProduct.ProductId);
            _Products.Remove(SelectedProduct);
            SelectedProduct = null;
        }

        private void SaveProduct()
        {
            if (SelectedProduct.ProductId != 0)
            {
                DataContext.Instance.Products.Update(SelectedProduct);
            }
            else
            {
                SelectedProduct.ProductId = DataContext.Instance.Products.Insert(SelectedProduct);
            }
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null)
            {
                return null;
            }

            using (var ms = new MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        private byte[] ConvertToByteArray(BitmapImage bitmapImage)
        {
            if (bitmapImage == null)
            {
                return null;
            }

            byte[] data;
            var encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private bool Validation()
        {
            if (SelectedProduct == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(SelectedProduct.Name))
            {
                return false;
            }

            return true;
        }

        private void ChangeSelectedProductCategory(SelectedProductCategory SelectedProductCategory)
        {
            ProductCategories = DataContext.Instance.ProductCategories.GetAll().ToSvenTechCollection();
            SelectedProduct.ProductCategory = SelectedProductCategory.ProductCategory;
            SelectedProduct.RefProductCategoryId = SelectedProductCategory.ProductCategory.ProductCategoryId;
            RaisePropertyChanged("SelectedProduct");
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<Product> FilteredProducts { get; set; } = new SvenTechCollection<Product>();
        public ProductStockingStatusViewModel ProductStockingStatusViewModel { get; set; } = new ProductStockingStatusViewModel();
        public SvenTechCollection<ProductCategory> ProductCategories { get; set; } =
            new SvenTechCollection<ProductCategory>();

        public SvenTechCollection<TaxType> TaxTypes => Globals.CoreData.TaxTypes;
        public DelegateCommand NewProductCommand { get; set; }
        public DelegateCommand SaveProductCommand { get; set; }
        public DelegateCommand DeleteProductCommand { get; set; }
        public DelegateCommand OpenProductCategoriesWindowCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredProducts = new SvenTechCollection<Product>();
                    foreach (var item in _Products)
                    {
                        if (item.Name.ToLower().Contains(FilterText.ToLower()))
                        {
                            FilteredProducts.Add(item);
                        }
                        else if (item.ItemNumber.ToString().Contains(FilterText))
                        {
                            FilteredProducts.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredProducts = _Products;
                }
            }
        }

        public Product SelectedProduct
        {
            get => _SelectedProduct;
            set
            {
                _SelectedProduct = value;
                ProductStockingStatusViewModel.Product = _SelectedProduct;
                if (_SelectedProduct != null && _SelectedProduct.Picture != null)
                {
                    Image = ConvertToImage(SelectedProduct.Picture);
                }
                else
                {
                    Image = null;
                }
            }
        }

        public BitmapImage Image
        {
            get => _Image;
            set
            {
                _Image = value;
                if (SelectedProduct != null)
                {
                    _SelectedProduct.Picture = ConvertToByteArray(value);
                }
            }
        }

        public User ActualUser => Globals.ActiveUser;

        #endregion Properties
    }
}