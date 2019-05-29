using DevExpress.Mvvm;

using FinancialAnalysis.Models.ProjectManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;
using WebApiWrapper.ProjectManagement;

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

        public SvenTechCollection<Employee> EmployeeList { get; set; } = new SvenTechCollection<Employee>();
        public SvenTechCollection<Employee> FilteredEmployees { get; set; } = new SvenTechCollection<Employee>();

        public SvenTechCollection<HealthInsurance> HealthInsuranceList { get; set; } =
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
            get => _FilterText;
            set
            {
                _FilterText = value;
                if (string.IsNullOrEmpty(_FilterText))
                {
                    FilteredEmployees = EmployeeList;
                }
                else
                {
                    FilteredEmployees = EmployeeList.Where(x => x.Name.ToLower().Contains(_FilterText.ToLower())).ToSvenTechCollection();
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
            EmployeeList.Add(SelectedEmployee);
        }

        private void DeleteUser()
        {
            if (SelectedEmployee == null)
            {
                return;
            }

            if (SelectedEmployee.EmployeeId == 0)
            {
                EmployeeList.Remove(SelectedEmployee);
                SelectedEmployee = null;
                return;
            }

            Employees.Delete(SelectedEmployee.EmployeeId);
            EmployeeList.Remove(SelectedEmployee);
            SelectedEmployee = null;
        }

        private void SaveUser()
        {
            if (SelectedEmployee.EmployeeId == 0)
            {
                SelectedEmployee.EmployeeId = Employees.Insert(SelectedEmployee);
            }
            else
            {
                Employees.Update(SelectedEmployee);
            }
            SelectedEmployee = null;
        }

        private void LoadEmployees()
        {
            FilteredEmployees = EmployeeList = Employees.GetAll().ToSvenTechCollection();
        }

        private void LoadHealthInsurances()
        {
            HealthInsuranceList = HealthInsurances.GetAll().ToSvenTechCollection();
        }

        private bool Validation()
        {
            if (SelectedEmployee == null)
            {
                return false;
            }

            return SelectedEmployee.IsValidForSaving;
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

        #endregion Methods
    }
}