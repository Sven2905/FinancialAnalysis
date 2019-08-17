using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysisActivationTool
{
    public class LicenseStringContainerViewModel : ViewModelBase
    {
        private string licenseString;
        public string LicenseString
        {
            get { return licenseString; }
            set { licenseString = value; RaisePropertyChanged(); }
        }

        //private void lnkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtLicense.Text))
        //    {
        //        Clipboard.SetText(txtLicense.Text);
        //    }
        //}

        //private void lnkSaveToFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    if (dlgSaveFile.ShowDialog() == DialogResult.OK)
        //    {
        //        //Save license data into local file
        //        File.WriteAllText(dlgSaveFile.FileName, txtLicense.Text.Trim(), Encoding.UTF8);
        //    }
        //}
    }
}
