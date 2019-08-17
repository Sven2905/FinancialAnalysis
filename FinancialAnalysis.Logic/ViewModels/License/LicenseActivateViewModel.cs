using DevExpress.Mvvm;
using License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LicenseActivateViewModel : ViewModelBase
    {
        public string AppName { get; set; }

        public byte[] CertificatePublicKeyData { private get; set; }

        public bool ShowMessageAfterValidation { get; set; }

        public Type LicenseObjectType { get; set; }

        public string UID { get; set; }
        public string License { get; set; }
        public string LicenseBASE64String
        {
            get
            {
                return License.Trim();
            }
        }

        public LicenseActivateViewModel()
        {
            ShowMessageAfterValidation = true;
        }

        public void ShowUID()
        {
            UID = LicenseHandler.GenerateUID(AppName);
        }

        public bool ValidateLicense()
        {
            if (string.IsNullOrWhiteSpace(License))
            {
                MessageBox.Show("Please input license", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Check the activation string
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            string _msg = string.Empty;
            LicenseEntity _lic = LicenseHandler.ParseLicenseFromBASE64String(LicenseObjectType, License.Trim(), CertificatePublicKeyData, out _licStatus, out _msg);
            switch (_licStatus)
            {
                case LicenseStatus.VALID:
                    if (ShowMessageAfterValidation)
                    {
                        MessageBox.Show(_msg, "License is valid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return true;

                case LicenseStatus.CRACKED:
                case LicenseStatus.INVALID:
                case LicenseStatus.UNDEFINED:
                    if (ShowMessageAfterValidation)
                    {
                        MessageBox.Show(_msg, "License is INVALID", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    return false;

                default:
                    return false;
            }
        }


        private void lnkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(UID);
        }
    }
}
