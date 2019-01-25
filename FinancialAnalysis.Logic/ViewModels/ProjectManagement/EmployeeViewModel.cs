using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        #region Constructor

        public EmployeeViewModel()
        {
            if (IsInDesignMode)
                return;

            LoadEmployees();
            LoadHealthInsurances();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteUserCommand = new DelegateCommand(DeleteUser, () => (SelectedEmployee != null));
        }


        #endregion Constructor

        #region Properties

        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();
        public SvenTechCollection<HealthInsurance> HealthInsurances { get; set; } = new SvenTechCollection<HealthInsurance>();
        public DelegateCommand NewUserCommand { get; set; } 
        public DelegateCommand SaveUserCommand { get; set; }
        public DelegateCommand DeleteUserCommand { get; set; }
        public Employee SelectedEmployee
        {
            get { return _SelectedEmployee; }
            set
            {
                _SelectedEmployee = value;
                if (_SelectedEmployee != null && _SelectedEmployee.Picture != null)
                {
                    Image = ConvertToImage(_SelectedEmployee.Picture);
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
                _Image = value; _SelectedEmployee.Picture = ConvertToByteArray(value);
            }
        }

        #endregion Properties

        #region Fields

        private Employee _SelectedEmployee;
        private BitmapImage _Image;

        #endregion Fields

        #region Methods

        private void NewUser()
        {
            SelectedEmployee = new Employee();
            Employees.Add(SelectedEmployee);
        }

        private void DeleteUser()
        {
            if (SelectedEmployee == null)
            {
                return;
            }

            if (SelectedEmployee.EmployeeId == 0)
            {
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
                return;
            }

            try
            {
                    DataLayer.Instance.Employees.Delete(SelectedEmployee.EmployeeId);
                    Employees.Remove(SelectedEmployee);
                    SelectedEmployee = null;
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveUser()
        {
            try
            {
                    if (SelectedEmployee.EmployeeId == 0)
                        SelectedEmployee.EmployeeId = DataLayer.Instance.Employees.Insert(SelectedEmployee);
                    else
                        DataLayer.Instance.Employees.Update(SelectedEmployee);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadEmployees()
        {
            try
            {
                    Employees = DataLayer.Instance.Employees.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void LoadHealthInsurances()
        {
            try
            {
                    HealthInsurances = DataLayer.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedEmployee == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedEmployee.Firstname) || string.IsNullOrEmpty(SelectedEmployee.Lastname) || string.IsNullOrEmpty(SelectedEmployee.Street)
                || string.IsNullOrEmpty(SelectedEmployee.City) || SelectedEmployee.Postcode == 0)
            {
                return false;
            }
            return true;
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

        #endregion Methods
    }
}