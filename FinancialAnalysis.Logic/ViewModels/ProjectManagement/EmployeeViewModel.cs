using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        #region Constructor

        public EmployeeViewModel()
        {
            if (IsInDesignMode) return;

            LoadEmployees();
            LoadHealthInsurances();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteUserCommand = new DelegateCommand(DeleteUser, () => SelectedEmployee != null);
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();

        public SvenTechCollection<HealthInsurance> HealthInsurances { get; set; } =
            new SvenTechCollection<HealthInsurance>();

        public DelegateCommand NewUserCommand { get; set; }
        public DelegateCommand SaveUserCommand { get; set; }
        public DelegateCommand DeleteUserCommand { get; set; }

        public Employee SelectedEmployee
        {
            get => _SelectedEmployee;
            set
            {
                _SelectedEmployee = value;
                if (_SelectedEmployee != null && _SelectedEmployee.Picture != null)
                    Image = ConvertToImage(_SelectedEmployee.Picture);
                else
                    Image = null;
            }
        }

        public BitmapImage Image
        {
            get => _Image;
            set
            {
                _Image = value;
                _SelectedEmployee.Picture = ConvertToByteArray(value);
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
            SelectedEmployee = new Employee
            {
                RefHealthInsuranceId = 1
            };
            Employees.Add(SelectedEmployee);
        }

        private void DeleteUser()
        {
            if (SelectedEmployee == null) return;

            if (SelectedEmployee.EmployeeId == 0)
            {
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
                return;
            }

            try
            {
                DataContext.Instance.Employees.Delete(SelectedEmployee.EmployeeId);
                Employees.Remove(SelectedEmployee);
                SelectedEmployee = null;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, MessageBoxImage.Error));
            }
        }

        private void SaveUser()
        {
            try
            {
                if (SelectedEmployee.EmployeeId == 0)
                    SelectedEmployee.EmployeeId = DataContext.Instance.Employees.Insert(SelectedEmployee);
                else
                    DataContext.Instance.Employees.Update(SelectedEmployee);
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, MessageBoxImage.Error));
            }
        }

        private void LoadEmployees()
        {
            try
            {
                Employees = DataContext.Instance.Employees.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, MessageBoxImage.Error));
            }
        }

        private void LoadHealthInsurances()
        {
            try
            {
                HealthInsurances = DataContext.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedEmployee == null) return false;
            if (string.IsNullOrEmpty(SelectedEmployee.Firstname) || string.IsNullOrEmpty(SelectedEmployee.Lastname) ||
                string.IsNullOrEmpty(SelectedEmployee.Street)
                || string.IsNullOrEmpty(SelectedEmployee.City) || SelectedEmployee.Postcode == 0)
                return false;
            return true;
        }

        public BitmapImage ConvertToImage(byte[] array)
        {
            if (array == null) return null;

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
            if (bitmapImage == null) return null;

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

        #endregion Methods
    }
}