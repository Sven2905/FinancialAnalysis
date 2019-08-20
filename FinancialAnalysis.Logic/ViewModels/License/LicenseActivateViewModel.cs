using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
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

        private string license;

        public string License
        {
            get { return license; }
            set { license = value; LicenseManager.Instance.License = value; }
        }
        public string UID { get; set; }


        public void ShowUID()
        {
            UID = LicenseManager.Instance.UID;
        }


        private void lnkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(UID);
        }
    }
}
