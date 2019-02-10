using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.ProjectManagement;
using System.IO;
using System.Linq;
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
            {
                return;
            }

            LoadEmployees();
            LoadHealthInsurances();
            NewUserCommand = new DelegateCommand(NewUser);
            SaveUserCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteUserCommand = new DelegateCommand(DeleteUser, () => SelectedEmployee != null);
        }

        #endregion Constructor

        #region Properties

        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();
        public SvenTechCollection<Employee> FilteredEmployees { get; set; } = new SvenTechCollection<Employee>();

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
                {
                    Image = ConvertToImage(_SelectedEmployee.Picture);
                }
                else
                {
                    Image = null;
                }
            }
        }

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (string.IsNullOrEmpty(_FilterText))
                {
                    FilteredEmployees = Employees;
                }
                else
                {
                    FilteredEmployees = Employees.Where(x => x.Name.ToLower().Contains(_FilterText.ToLower())).ToSvenTechCollection();
                }
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

        private string _FilterText;
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

            DataContext.Instance.Employees.Delete(SelectedEmployee.EmployeeId);
            Employees.Remove(SelectedEmployee);
            SelectedEmployee = null;
        }

        private void SaveUser()
        {
            if (SelectedEmployee.EmployeeId == 0)
            {
                SelectedEmployee.EmployeeId = DataContext.Instance.Employees.Insert(SelectedEmployee);
            }
            else
            {
                DataContext.Instance.Employees.Update(SelectedEmployee);
            }
            SelectedEmployee = null;
        }

        private void LoadEmployees()
        {
            FilteredEmployees = Employees = DataContext.Instance.Employees.GetAll().ToSvenTechCollection();
        }

        private void LoadHealthInsurances()
        {
            HealthInsurances = DataContext.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
        }

        private bool Validation()
        {
            if (SelectedEmployee == null)
            {
                return false;
            }

            return SelectedEmployee.ValidForSaving;
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

        #endregion Methods
    }
}