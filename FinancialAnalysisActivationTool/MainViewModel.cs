using DevExpress.Mvvm;
using License;
using Licenses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysisActivationTool
{
    public class MainViewModel : ViewModelBase
    {
        private byte[] _certPubicKeyData;
        private SecureString _certPwd = new SecureString();

        private LicenseSettingsViewModel licenseSettingsViewModel = new LicenseSettingsViewModel();
        private LicenseStringContainerViewModel licenseStringContainerViewModel = new LicenseStringContainerViewModel();

        public LicenseSettingsViewModel LicenseSettingsViewModel
        {
            get { return licenseSettingsViewModel; }
            set { licenseSettingsViewModel = value; RaisePropertyChanged(); }
        }

        public LicenseStringContainerViewModel LicenseStringContainerViewModel
        {
            get { return licenseStringContainerViewModel; }
            set { licenseStringContainerViewModel = value; RaisePropertyChanged(); }
        }

        public MainViewModel()
        {
            _certPwd.AppendChar('d');
            _certPwd.AppendChar('e');
            _certPwd.AppendChar('m');
            _certPwd.AppendChar('o');

            Initialize();
        }

        private void Initialize()
        {
            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("FinancialAnalysisActivationTool.LicenseSign.pfx").CopyTo(_mem);
                _certPubicKeyData = _mem.ToArray();
            }

            //Initialize the path for the certificate to sign the XML license file
            LicenseSettingsViewModel.CertificatePrivateKeyData = _certPubicKeyData;
            LicenseSettingsViewModel.CertificatePassword = _certPwd;

            //Initialize a new license object
            LicenseSettingsViewModel.License = new FinancialAnalysisLicense();
            LicenseSettingsViewModel.OnLicenseGenerated += LicenseSettingsViewModel_OnLicenseGenerated;
        }

        private void LicenseSettingsViewModel_OnLicenseGenerated(object sender, LicenseGeneratedEventArgs e)
        {
            //Event raised when license string is generated. Just show it in the text box
            licenseStringContainerViewModel.LicenseString = e.LicenseBASE64String;
        }

        private void btnGenSvrMgmLic_Click(object sender, EventArgs e)
        {
            //Event raised when "Generate License" button is clicked. 
            //Call the core library to generate the license
            licenseStringContainerViewModel.LicenseString = LicenseHandler.GenerateLicenseBASE64String(
                new FinancialAnalysisLicense(),
                _certPubicKeyData,
                _certPwd);
        }
    }
}
