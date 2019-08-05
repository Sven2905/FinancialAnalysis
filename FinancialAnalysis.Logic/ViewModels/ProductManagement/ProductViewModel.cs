using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Utilities;
using WebApiWrapper.ProductManagement;

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
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.ProductCategories) ||
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
            ProductCategoryList = LoadAllProductCategories();
        }

        private void OpenProductCategoriesWindow()
        {
            Messenger.Default.Send(new OpenProductCategoriesWindowMessage());
        }

        private SvenTechCollection<Product> LoadAllProducts()
        {
            SvenTechCollection<Product> allProducts = new SvenTechCollection<Product>();
            return Products.GetAll().ToSvenTechCollection();
        }

        private SvenTechCollection<ProductCategory> LoadAllProductCategories()
        {
            SvenTechCollection<ProductCategory> allProductCategories = new SvenTechCollection<ProductCategory>();
            return ProductCategories.GetAll().ToSvenTechCollection();
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

            Products.Delete(SelectedProduct.ProductId);
            _Products.Remove(SelectedProduct);
            SelectedProduct = null;
        }

        private void SaveProduct()
        {
            if (SelectedProduct.ProductId != 0)
            {
                Products.Update(SelectedProduct);
            }
            else
            {
                SelectedProduct.ProductId = Products.Insert(SelectedProduct);
            }
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null)
            {
                return null;
            }

            using (MemoryStream ms = new MemoryStream(array))
            {
                BitmapImage image = new BitmapImage();
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
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage, null, null, null));
            using (MemoryStream ms = new MemoryStream())
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
            ProductCategoryList = ProductCategories.GetAll().ToSvenTechCollection();
            SelectedProduct.ProductCategory = SelectedProductCategory.ProductCategory;
            SelectedProduct.RefProductCategoryId = SelectedProductCategory.ProductCategory.ProductCategoryId;
            RaisePropertyChanged("SelectedProduct");
        }

        #endregion Methods

        #region Properties

        public ItemPriceCalculationViewModel ItemPriceCalculationViewModel { get; set; } = new ItemPriceCalculationViewModel();
        public SvenTechCollection<Product> FilteredProducts { get; set; } = new SvenTechCollection<Product>();
        public ProductStockingStatusViewModel ProductStockingStatusViewModel { get; set; } = new ProductStockingStatusViewModel();

        public SvenTechCollection<ProductCategory> ProductCategoryList { get; set; } =
            new SvenTechCollection<ProductCategory>();

        public SvenTechCollection<TaxType> TaxTypes => Globals.CoreData.TaxTypeList;
        public DelegateCommand NewProductCommand { get; set; }
        public DelegateCommand SaveProductCommand { get; set; }
        public DelegateCommand DeleteProductCommand { get; set; }
        public DelegateCommand OpenProductCategoriesWindowCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;
        public bool AllowItemCalculation => SelectedProduct != null && SelectedProduct.ProductId != 0;

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredProducts = new SvenTechCollection<Product>();
                    foreach (Product item in _Products)
                    {
                        if (item.Name.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0)
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
                if (_SelectedProduct != null)
                {
                    if (_SelectedProduct.Picture != null)
                        Image = ConvertToImage(SelectedProduct.Picture);
                    ItemPriceCalculationViewModel.Product = _SelectedProduct;
                    ItemPriceCalculationViewModel.ItemPriceCalculationInputItem.ProductionMaterial = _SelectedProduct.DefaultBuyingPrice;
                    ItemPriceCalculationViewModel.Refresh();
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