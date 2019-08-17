using DevExpress.Mvvm;
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
        byte[] _certPubicKeyData;
        public LicenseActivateViewModel LicenseActivateViewModel { get; set; } = new LicenseActivateViewModel();

        public LicenseViewModel()
        {
            Initialize();
            ValidateCommand = new DelegateCommand(Validation);
            //Assign the application information values to the license control
            LicenseActivateViewModel.AppName = "FinancialAnalysis";
            LicenseActivateViewModel.LicenseObjectType = typeof(Licenses.FinancialAnalysisLicense);
            LicenseActivateViewModel.CertificatePublicKeyData = _certPubicKeyData;
            //Display the device unique ID
            LicenseActivateViewModel.ShowUID();
        }


        private void Validation()
        {
            //Call license control to validate the license string
            if (LicenseActivateViewModel.ValidateLicense())
            {
                //If license if valid, save the license string into a local file
                File.WriteAllText(Path.Combine(Application.StartupPath, "license.lic"), LicenseActivateViewModel.LicenseBASE64String);

                MessageBox.Show("Lizenz akzeptiert. Die Applikation wird neugestartet.", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
        }

        private void Initialize()
        {
            //Initialize variables with default values
            FinancialAnalysisLicense _lic = null;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("FinancialAnalysis.Logic.LicenseVerify.cer").CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (FinancialAnalysisLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(FinancialAnalysisLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
                Globals.ActiveLicense = _lic;
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            if (_status == LicenseStatus.VALID)
            {
                //TODO: If license is valid, you can do extra checking here
                //TODO: E.g., check license  if you have added expiry date property to your license entity
                //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                //Here for demo, just show the license information and RETURN without additional checking       
                //licInfo.ShowLicenseInfo(_lic);

                return;
            }
            else
            {
                LicenseActivateViewModel.CertificatePublicKeyData = _certPubicKeyData;
            }
        }
    }
}
