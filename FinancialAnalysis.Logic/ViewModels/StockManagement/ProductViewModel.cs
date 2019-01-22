using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.Product;
using System.IO;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        #region Fields

        private ProductPrototype _SelectedProduct;
        private BitmapImage _Image;
        private SvenTechCollection<ProductPrototype> _Products = new SvenTechCollection<ProductPrototype>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public ProductViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _Products = LoadAllProducts();
            ProductCategories = LoadAllProductCategories();
            NewProductCommand = new DelegateCommand(NewProduct);
            SaveProductCommand = new DelegateCommand(SaveProduct, () => Validation());
            DeleteProductCommand = new DelegateCommand(DeleteProduct, () => (SelectedProduct != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<ProductPrototype> LoadAllProducts()
        {
            SvenTechCollection<ProductPrototype> allProducts = new SvenTechCollection<ProductPrototype>();
            try
            {
                using (var db = new DataLayer())
                {
                    allProducts = db.ProductPrototypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allProducts;
        }

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

        private void NewProduct()
        {
            SelectedProduct = new ProductPrototype();
            _Products.Add(SelectedProduct);
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            if (SelectedProduct.ProductPrototypeId == 0)
            {
                _Products.Remove(SelectedProduct);
                SelectedProduct = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.ProductPrototypes.Delete(SelectedProduct.ProductPrototypeId);
                    _Products.Remove(SelectedProduct);
                    SelectedProduct = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveProduct()
        {
            try
            {
                if (SelectedProduct.ProductPrototypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.ProductPrototypes.Update(SelectedProduct);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        SelectedProduct.ProductPrototypeId = db.ProductPrototypes.Insert(SelectedProduct);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null)
            {
                return null;
            }

            using (var ms = new System.IO.MemoryStream(array))
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
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
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

        #endregion Methods

        #region Properties

        public SvenTechCollection<ProductPrototype> FilteredProducts { get; set; } = new SvenTechCollection<ProductPrototype>();
        public SvenTechCollection<ProductCategory> ProductCategories { get; set; } = new SvenTechCollection<ProductCategory>();
        public DelegateCommand NewProductCommand { get; set; }
        public DelegateCommand SaveProductCommand { get; set; }
        public DelegateCommand DeleteProductCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredProducts = new SvenTechCollection<ProductPrototype>();
                    foreach (var item in _Products)
                    {
                        if (item.Name.Contains(FilterText))
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
        public ProductPrototype SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
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
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                if (SelectedProduct != null)
                {
                    _SelectedProduct.Picture = ConvertToByteArray(value);
                }
            }
        }
        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
