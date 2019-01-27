using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic;
using FinancialAnalysis.Models.CompanyManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MyCompanyViewModel : ViewModelBase
    {
        #region Constructor

        public MyCompanyViewModel()
        {
            if (IsInDesignMode)
                return;

            Company = Globals.CoreData.MyCompany;
            SaveCompanyCommand = new DelegateCommand(SaveCompany, Validation);
        }

        #endregion Constructor

        #region Fields

        private Company _Company = new Company();
        private BitmapImage _Image;

        #endregion Fields

        #region Properties

        public Company Company
        {
            get { return _Company; }
            set
            {
                _Company = value;
                if (_Company != null && _Company.Logo != null)
                {
                    Image = ConvertToImage(_Company.Logo);
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
                _Image = value; _Company.Logo = ConvertToByteArray(value);
            }
        }

        public DelegateCommand SaveCompanyCommand { get; set; }

        #endregion Properties

        #region Methods

        private void SaveCompany()
        {
            DataLayer.Instance.Companies.Update(Company);
            Globals.CoreData.RefreshData();
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
            if (Company == null)
                return false;

            return (!string.IsNullOrEmpty(Company.Name) && !string.IsNullOrEmpty(Company.Street) && !string.IsNullOrEmpty(Company.City));
        }

        #endregion Methods
    }
}
