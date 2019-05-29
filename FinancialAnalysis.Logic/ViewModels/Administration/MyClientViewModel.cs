using DevExpress.Mvvm;
using FinancialAnalysis.Models.ClientManagement;
using System.IO;
using System.Windows.Media.Imaging;
using WebApiWrapper.ClientManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MyClientViewModel : ViewModelBase
    {
        #region Constructor

        public MyClientViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            Client = Globals.CoreData.MyCompany;
            SaveClientCommand = new DelegateCommand(SaveClient, Validation);
        }

        #endregion Constructor

        #region Fields

        private Client _Client = new Client();
        private BitmapImage _Image;

        #endregion Fields

        #region Properties

        public Client Client
        {
            get => _Client;
            set
            {
                _Client = value;
                if (_Client != null && _Client.Company.Logo != null)
                {
                    Image = ConvertToImage(_Client.Company.Logo);
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
                _Client.Company.Logo = ConvertToByteArray(value);
            }
        }

        public DelegateCommand SaveClientCommand { get; set; }

        #endregion Properties

        #region Methods

        private void SaveClient()
        {
            Companies.Update(Client.Company);
            Clients.Update(Client);
            Globals.CoreData.RefreshData();
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
            if (Client == null)
            {
                return false;
            }

            return !string.IsNullOrEmpty(Client.Name) && !string.IsNullOrEmpty(Client.Street) &&
                   !string.IsNullOrEmpty(Client.City);
        }

        #endregion Methods
    }
}