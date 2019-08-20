using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using License;
using Licenses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LicenseViewModel : ViewModelBase
    {
        public DelegateCommand ValidateCommand { get; set; }

        public LicenseActivateViewModel LicenseActivateViewModel { get; set; } = new LicenseActivateViewModel();

        public LicenseViewModel()
        {
            ValidateCommand = new DelegateCommand(Validation);
            //Assign the application information values to the license control
            //Display the device unique ID
            LicenseActivateViewModel.ShowUID();
        }

        private void Validation()
        {
            LicenseManager.Instance.Validation();
        }
    }
}
