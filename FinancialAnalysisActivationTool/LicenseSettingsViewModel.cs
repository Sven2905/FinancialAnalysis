using DevExpress.Mvvm;
using License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalysisActivationTool
{
    public delegate void LicenseSettingsValidatingHandler(object sender, LicenseSettingsValidatingEventArgs e);
    public delegate void LicenseGeneratedHandler(object sender, LicenseGeneratedEventArgs e);

    public class LicenseSettingsViewModel : ViewModelBase
    {
        public event LicenseSettingsValidatingHandler OnLicenseSettingsValidating;
        public event LicenseGeneratedHandler OnLicenseGenerated;

        public LicenseSettingsViewModel()
        {
            GenerateCommand = new DelegateCommand(GenerateLicense, _lic != null);
        }

        protected LicenseEntity _lic;
        private bool allowVolumeLicense;
        private LicenseEntity selectedLicense;

        public DelegateCommand GenerateCommand { get; set; }
        public LicenseEntity SelectedLicense
        {
            get { return selectedLicense; }
            set { selectedLicense = value; RaisePropertyChanged(); }
        }


        public LicenseEntity License
        {
            set
            {
                _lic = value;
                SelectedLicense = _lic;
            }
        }

        public byte[] CertificatePrivateKeyData { set; private get; }

        public SecureString CertificatePassword { set; private get; }

        public bool IsVolumeLicense
        {
            get { return allowVolumeLicense; }
            set { allowVolumeLicense = value; RaisePropertyChanged(); }
        }

        private string uid;

        public string UID
        {
            get { return uid; }
            set { uid = value; RaisePropertyChanged(); }
        }


        private void LicenseTypeRadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            UID = string.Empty;
        }

        private void GenerateLicense()
        {
            if (string.IsNullOrEmpty(UID))
            {
                MessageBox.Show("Bitte Lizenz UID eingeben.", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!IsVolumeLicense)
            {
                if (LicenseHandler.ValidateUIDFormat(UID.Trim()))
                {
                    _lic.Type = LicenseTypes.Single;
                    _lic.UID = UID.Trim();
                }
                else
                {
                    MessageBox.Show("License UID is blank or invalid", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (IsVolumeLicense)
            {
                _lic.Type = LicenseTypes.Volume;
                _lic.UID = string.Empty;
            }

            _lic.CreateDateTime = DateTime.Now;

            if (OnLicenseSettingsValidating != null)
            {
                LicenseSettingsValidatingEventArgs _args = new LicenseSettingsValidatingEventArgs() { License = _lic, CancelGenerating = false };

                OnLicenseSettingsValidating(this, _args);

                if (_args.CancelGenerating)
                {
                    return;
                }
            }

            if (OnLicenseGenerated != null)
            {
                string _licStr = LicenseHandler.GenerateLicenseBASE64String(_lic, CertificatePrivateKeyData, CertificatePassword);

                OnLicenseGenerated(this, new LicenseGeneratedEventArgs() { LicenseBASE64String = _licStr });
            }
        }
    }
}
